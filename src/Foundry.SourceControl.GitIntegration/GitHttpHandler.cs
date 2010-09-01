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
        private static Dictionary<string, Dictionary<Regex, Action<IGitSession, string, HttpContext>>> _services = 
            new Dictionary<string,Dictionary<Regex,Action<IGitSession, string, HttpContext>>>()
            {
                { "GET", new Dictionary<Regex, Action<IGitSession, string, HttpContext>>
                    { 
                        { new Regex(".*?git-upload-pack$", RegexOptions.Compiled), HandleUploadPackGet },
                        { new Regex(".*?git-receive-pack$", RegexOptions.Compiled), HandleReceivePackGet }
                    }
                },
                { "POST", new Dictionary<Regex, Action<IGitSession, string, HttpContext>>
                    {
                        { new Regex(".*?git-upload-pack$", RegexOptions.Compiled), HandleUploadPackPost },
                        { new Regex(".*?git-receive-pack$", RegexOptions.Compiled), HandleReceivePackPost }
                    }
                }
            };

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string repo = GetRepo(context.Request.RawUrl);

            if (string.IsNullOrEmpty(repo) ||
                !Directory.Exists(Path.Combine(GitSettings.RepositoriesPath, repo)))
            {
                context.Response.StatusCode = 400;
                context.Response.End();
                return;
            }

            using (var gitSession = new GitSession(GitSettings.ExePath, GitSettings.RepositoriesPath))
            {
                var handlers = _services[context.Request.RequestType];

                KeyValuePair<Regex, Action<IGitSession, string, HttpContext>>? handler = handlers.FirstOrDefault(x => x.Key.IsMatch(context.Request.RawUrl));

                if (handler == null)
                {
                    context.Response.StatusCode = 400;
                    context.Response.End();
                    return;
                }

                handler.Value.Value(gitSession, repo, context);
            }
        }

        private static void HandleReceivePackGet(IGitSession session, string repository, HttpContext context)
        {
            var cmd = new GitSmartHttpReceivePackCommand(session, repository) { AdvertiseRefs = true };
            ExecuteInfoRefsCommand(context, cmd);
        }

        private static void HandleReceivePackPost(IGitSession session, string repository, HttpContext context)
        {
            try
            {
                var cmd = new GitSmartHttpReceivePackCommand(session, repository);
                ExecuteServiceCommand(context, cmd);
            }
            finally
            {
                var cmd = new GitUpdateServerInfoCommand(session);
                cmd.Execute();
            }
        }

        private static void HandleUploadPackGet(IGitSession session, string repository, HttpContext context)
        {
            var cmd = new GitSmartHttpUploadPackCommand(session, repository) { AdvertiseRefs = true };
            ExecuteInfoRefsCommand(context, cmd);
        }

        private static void HandleUploadPackPost(IGitSession session, string repository, HttpContext context)
        {
            var cmd = new GitSmartHttpUploadPackCommand(session, repository);
            ExecuteServiceCommand(context, cmd);
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
            context.Response.End();
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

        
    }
}