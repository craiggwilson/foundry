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
                        { new Regex("(.*?)git-upload-pack$", RegexOptions.Compiled), HandleUploadPackGet },
                        { new Regex("(.*?)git-receive-pack$", RegexOptions.Compiled), HandleReceivePackGet },
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
                        { new Regex("(.*?)git-upload-pack$", RegexOptions.Compiled), HandleUploadPackPost },
                        { new Regex("(.*?)git-receive-pack$", RegexOptions.Compiled), HandleReceivePackPost }
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
                    if (context.Request.RequestType == "PROPFIND")
                        context.Response.StatusCode = 405;
                    else
                        context.Response.StatusCode = 400;
                    context.Response.End();
                    return;
                }

                route.Handler(context, route);
            }
            context.Response.End();
        }

        private static void HandleReceivePackGet(HttpContext context, GitRoute route)
        {
            using (var session = CreateGitSession())
            {
                var cmd = new GitSmartHttpReceivePackCommand(session, route.Repository) { AdvertiseRefs = true };
                ExecuteInfoRefsCommand(context, cmd);
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

        private static void HandleUploadPackGet(HttpContext context, GitRoute route)
        {
            using (var session = CreateGitSession())
            {
                var cmd = new GitSmartHttpUploadPackCommand(session, route.Repository) { AdvertiseRefs = true };
                ExecuteInfoRefsCommand(context, cmd);
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
            var file = Path.Combine(GitSettings.RepositoriesPath, route.Repository, route.File);
            context.Response.ContentType = contentType;
            context.Response.AddFileDependency(file);

            context.Response.WriteFile(file);
        }

        private static void ExecuteCommand(HttpContext context, IGitCommand cmd, bool readFile)
        {
            context.Response.ContentEncoding = Encoding.Unicode;
            using (var outputWriter = new StreamWriter(context.Response.OutputStream, context.Response.ContentEncoding))
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
        }

        private static void ExecuteInfoRefsCommand(HttpContext context, IGitCommand cmd)
        {
            context.Response.ContentType = string.Format("application/x-git-{0}-advertisement", cmd.Name);
            context.Response.Write(GitString("# service=git-" + cmd.Name));
            context.Response.Write("0000");
            ExecuteCommand(context, cmd, false);
        }

        private static void ExecuteServiceCommand(HttpContext context, IGitCommand cmd)
        {
            context.Response.ContentType = string.Format("application/x-git-{0}-result", cmd.Name);
            ExecuteCommand(context, cmd, true);
        }

        private static string GetRepo(string url)
        {
            var match = Regex.Match(url, @".*?([^\/]+\/.+?)\.git.*");
            if (!match.Success)
                return "";

            return match.Groups[1].Value;
        }

        private static string GitString(string s)
        {
            var len = (s.Length + 4).ToString("x");
            while (len.Length < 4) 
                len = "0" + len;
            return len + s;
        }

        private static GitRoute GetGitRoute(HttpContext context)
        {
            if (!_services.ContainsKey(context.Request.RequestType))
                return null;

            var handlers = _services[context.Request.RequestType];
            foreach(var pair in handlers)
            {
                Match m = pair.Key.Match(context.Request.RawUrl);
                if(m.Success)
                {
                    var route = new GitRoute();
                    route.Handler = pair.Value;
                    route.Repository = context.Request.FilePath.Replace(context.Request.ApplicationPath + "/", "").Replace(".git", "");
                    route.File = context.Request.RawUrl.Replace(m.Groups[1].Value + "/", "");
                    return route;
                }
            }

            return null;
        }

        private static IGitSession CreateGitSession()
        {
            return new GitSession(GitSettings.ExePath, GitSettings.RepositoriesPath);
        }

        private class GitRoute
        {
            public Action<HttpContext, GitRoute> Handler;
            public string Repository;
            public string File;
        }
        
    }
}