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
                        { new Regex(".*?/git-upload-pack$", RegexOptions.Compiled), HandleUploadPackGet },
                        { new Regex(".*?/git-receive-pack$", RegexOptions.Compiled), HandleReceivePackGet }
                    }
                },
                { "POST", new Dictionary<Regex, Action<IGitSession, string, HttpContext>>
                    {
                        { new Regex(".*?/git-upload-pack$", RegexOptions.Compiled), HandleUploadPackPost },
                        { new Regex(".*?/git-receive-pack$", RegexOptions.Compiled), HandleReceivePackPost }
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
            repo = Path.Combine(GitSettings.RepositoriesPath, repo);

            if (string.IsNullOrEmpty(repo) ||
                !Directory.Exists(repo))
            {
                context.Response.StatusCode = 400;
                context.Response.End();
                return;
            }

            using (var gitSession = new GitSession(GitSettings.ExePath, GitSettings.RepositoriesPath))
            {
                var handlers = _services[context.Request.RequestType];

                Action<IGitSession, string, HttpContext> handler = null;

                foreach (var kvp in handlers)
                {
                    if (kvp.Key.IsMatch(context.Request.RawUrl))
                    {
                        handler = kvp.Value;
                        break;
                    }
                }

                if (handler == null)
                {
                    throw new NotSupportedException();
                }

                handler(gitSession, repo, context);
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
            cmd.Outfile = Path.GetTempFileName();

            if (readFile)
            {
                cmd.Infile = Path.GetTempFileName();
                using (var file = File.Create(cmd.Infile))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = context.Request.InputStream.Read(buffer, 0, buffer.Length)) != 0)
                        file.Write(buffer, 0, bytesRead);
                }
            }

            var result = cmd.Execute();

            context.Response.WriteFile(cmd.Outfile);
            context.Response.End();

            File.Delete(cmd.Outfile);
            if(readFile)
                File.Delete(cmd.Infile);
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