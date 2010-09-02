using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using Foundry.SourceControl.GitIntegration.Commands;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitHttpHandler : IHttpHandler
    {
        private static Dictionary<string, Dictionary<Regex, Action<HttpContext, GitRoute>>> _services =
            new Dictionary<string, Dictionary<Regex, Action<HttpContext, GitRoute>>>()
            {
                { "GET", new Dictionary<Regex, Action<HttpContext, GitRoute>>
                    { 
                        { new Regex("(.*?)/info/refs$", RegexOptions.Compiled), HandleInfoRefsGet },
                        { new Regex("(.*?)/HEAD$", RegexOptions.Compiled), (c, r) => SendFile(c, r, "text/plain") },
                        { new Regex("(.*?)/objects/info/alternates$", RegexOptions.Compiled), (c, r) => SendFile(c, r, "text/plain") },
                        { new Regex("(.*?)/objects/info/http-alternates$", RegexOptions.Compiled), (c, r) => SendFile(c, r, "text/plain") },
                        { new Regex("(.*?)/objects/info/packs$", RegexOptions.Compiled), (c, r) => SendFile(c, r, "text/plain; charset=utf-8") },
                        { new Regex("(.*?)/objects/info/[^/]*$", RegexOptions.Compiled),  (c, r) => SendFile(c, r, "text/plain") },
                        { new Regex("(.*?)/objects/[0-9a-f]{2}/[0-9a-f]{38}$", RegexOptions.Compiled), (c, r) => SendFile(c, r, "application/x-git-loose-object") },
                        { new Regex("(.*?)/objects/pack/pack-[0-9a-f]{40}\\.pack$", RegexOptions.Compiled), (c, r) => SendFile(c, r, "application/x-git-packed-objects") },
                        { new Regex("(.*?)/objects/pack/pack-[0-9a-f]{40}\\.idx$", RegexOptions.Compiled), (c, r) => SendFile(c, r, "application/x-git-packed-objects-toc") }
                    }
                },
                { "POST", new Dictionary<Regex, Action<HttpContext, GitRoute>>
                    {
                        { new Regex("(.*?)/git-upload-pack$", RegexOptions.Compiled), HandleUploadPackPost },
                        { new Regex("(.*?)/git-receive-pack$", RegexOptions.Compiled), HandleReceivePackPost }
                    }
                }
            };

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            using (var gitSession = new GitSession(GitSettings.ExePath, GitSettings.RepositoriesPath))
            {
                var route = GetGitRoute(context);
                if (route == null)
                {
                    Respond404(context);
                    return;
                }

                route.Handler(context, route);
            }
            context.Response.End();
        }

        private static void HandleInfoRefsGet(HttpContext context, GitRoute route)
        {
            var service = context.Request.QueryString["service"];

            if (string.IsNullOrWhiteSpace(service) || !service.StartsWith("git-"))
            {
                SendFile(context, route, "text/plain; charset=utf-8");
            }
            else
            {
                using (var session = CreateGitSession())
                {
                    var cmd = new RawGitCommand(session, service.Substring(4));
                    cmd.Arguments.Add("--stateless-rpc");
                    cmd.Arguments.Add("--advertise-refs");
                    cmd.Arguments.Add(route.Repository);
                    ExecuteInfoRefsCommand(context, cmd);
                }
            }
        }

        private static void HandleReceivePackPost(HttpContext context, GitRoute route)
        {
            using (var session = CreateGitSession())
            {
                try
                {
                    var cmd = new GitSmartHttpReceivePackCommand(session, route.Repository);
                    ExecuteServiceCommand(context, cmd);
                }
                finally
                {
                    var cmd = new GitUpdateServerInfoCommand(session);
                    cmd.Execute();
                }
            }
        }

        private static void HandleUploadPackPost(HttpContext context, GitRoute route)
        {
            using (var session = CreateGitSession())
            {
                var cmd = new GitSmartHttpUploadPackCommand(session, route.Repository);
                ExecuteServiceCommand(context, cmd);
            }
        }

        private static void SendFile(HttpContext context, GitRoute route, string contentType)
        {
            var file = new FileInfo(route.FilePath);
            if (!file.Exists)
                return;

            context.Response.ContentType = contentType;
            context.Response.AddFileDependency(file.FullName);
            context.Response.WriteFile(file.FullName);
        }

        private static void ExecuteCommand(HttpContext context, IGitCommand cmd, bool readFile)
        {
            var file = Path.GetTempFileName();
            using(var stream = File.Create(file))
            using (var outputWriter = new StreamWriter(stream))
            {
                cmd.Output = outputWriter;

                try
                {
                    if (readFile)
                        cmd.Input = new StreamReader(context.Request.InputStream);

                    cmd.Execute();
                }
                finally
                {
                    if (readFile)
                        cmd.Input.Dispose();
                }
            }

            context.Response.WriteFile(file);
            try
            {
                File.Delete(file);
            }
            catch { }
        }

        private static void ExecuteInfoRefsCommand(HttpContext context, IGitCommand cmd)
        {
            context.Response.ContentType = string.Format("application/x-git-{0}-advertisement", cmd.Name);
            context.Response.Write(GitString("# service=git-" + cmd.Name + "\n"));
            context.Response.Write("0000");
            ExecuteCommand(context, cmd, false);
        }

        private static void ExecuteServiceCommand(HttpContext context, IGitCommand cmd)
        {
            context.Response.ContentType = string.Format("application/x-git-{0}-result", cmd.Name);
            ExecuteCommand(context, cmd, true);
        }

        private static string GitString(string s)
        {
            return (s.Length + 4).ToString("x").PadLeft(4, '0') + s;
        }

        private static GitRoute GetGitRoute(HttpContext context)
        {
            if (!_services.ContainsKey(context.Request.RequestType))
                return null;

            var handlers = _services[context.Request.RequestType];
            foreach (var pair in handlers)
            {
                Match m = pair.Key.Match(context.Request.Path);
                if (m.Success)
                {
                    var route = new GitRoute();
                    route.Handler = pair.Value;
                    if (context.Request.ApplicationPath == "/")
                        route.Repository = context.Request.FilePath.Substring(1, context.Request.FilePath.Length - 5);
                    else
                        route.Repository = context.Request.FilePath.Replace(context.Request.ApplicationPath + "/", "").Replace(".git", "");
                    route.File = context.Request.Path.Replace(m.Groups[1].Value + "/", "");

                    route.RepositoryPath = Path.Combine(GitSettings.RepositoriesPath, route.Repository);
                    route.FilePath = Path.Combine(route.RepositoryPath, route.File);
                    return route;
                }
            }

            return null;
        }

        private static IGitSession CreateGitSession()
        {
            return new GitSession(GitSettings.ExePath, GitSettings.RepositoriesPath);
        }

        private static void Respond404(HttpContext context)
        {
            context.Response.StatusCode = 400;
            context.Response.End();
        }

        private class GitRoute
        {
            public Action<HttpContext, GitRoute> Handler;
            public string RepositoryPath;
            public string Repository;
            public string File;
            public string FilePath;
        }

    }
}