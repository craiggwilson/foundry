using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using Foundry.SourceControl.GitIntegration;

using GitSharp.Core;
using GitSharp.Core.Transport;

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
            var route = GetGitRoute(context);
            if (route == null)
            {
                Respond404(context);
                return;
            }

            route.Handler(context, route);
        }

        private static void HandleInfoRefsGet(HttpContext context, GitRoute route)
        {
            var gitService = context.Request.QueryString["service"];

            if (string.IsNullOrWhiteSpace(gitService) || !gitService.StartsWith("git-"))
            {
                SendFile(context, route, "text/plain; charset=utf-8");
            }
            else
            {
                var service = gitService.Substring(4);
                context.Response.ContentType = string.Format("application/x-git-{0}-advertisement", service);
                context.Response.Write(GitString("# service=git-" + service + "\n"));
                context.Response.Write("0000");

                if (service == "upload-pack")
                {
                    var pack = new UploadPack(route.Repository);
                    pack.sendAdvertisedRefs(new RefAdvertiser.PacketLineOutRefAdvertiser(new PacketLineOut(context.Response.OutputStream)));
                }
                else if (service == "receive-pack")
                {
                    VerifyAccess(context);
                    var pack = new ReceivePack(route.Repository);
                    pack.SendAdvertisedRefs(new RefAdvertiser.PacketLineOutRefAdvertiser(new PacketLineOut(context.Response.OutputStream)));
                }
            }
        }

        private static void HandleReceivePackPost(HttpContext context, GitRoute route)
        {
            VerifyAccess(context);
            context.Response.ContentType = "application/x-git-receive-pack-result";
            var pack = new ReceivePack(route.Repository);
            pack.setBiDirectionalPipe(false);
            pack.receive(context.Request.InputStream, context.Response.OutputStream, context.Response.OutputStream);
        }

        private static void HandleUploadPackPost(HttpContext context, GitRoute route)
        {
            context.Response.ContentType = "application/x-git-upload-pack-result";
            var pack = new UploadPack(route.Repository);
            pack.setBiDirectionalPipe(false);
            pack.Upload(context.Request.InputStream, context.Response.OutputStream, context.Response.OutputStream);
        }

        private static void SendFile(HttpContext context, GitRoute route, string contentType)
        {
            var file = new FileInfo(route.FilePath);
            context.Response.StatusCode = 200;
            context.Response.ContentType = contentType;
            if (!file.Exists)
                return;

            context.Response.AddFileDependency(file.FullName);
            context.Response.WriteFile(file.FullName);
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

                    string repo;
                    if (context.Request.ApplicationPath == "/")
                        repo = context.Request.FilePath.Substring(1, context.Request.FilePath.Length - 5);
                    else
                        repo = context.Request.FilePath.Replace(context.Request.ApplicationPath + "/", "");
                    route.File = context.Request.Path.Replace(m.Groups[1].Value + "/", "");

                    route.Repository = Repository.Open(new DirectoryInfo(Path.Combine(GitSettings.RepositoriesPath, repo)));
                    route.FilePath = Path.Combine(route.Repository.Directory.FullName, route.File);
                    return route;
                }
            }

            return null;
        }

        private static void VerifyAccess(HttpContext context)
        {
            //if (!context.User.Identity.IsAuthenticated)
            //{
            //    context.Response.StatusCode = 401;
            //    context.Response.StatusDescription = "Access Denied";
            //    context.Response.Write("401 Access Denied");
            //    context.Response.End();
            //}
        }

        private static void Respond404(HttpContext context)
        {
            context.Response.StatusCode = 404;
            context.Response.End();
        }

        private class GitRoute
        {
            public Action<HttpContext, GitRoute> Handler;
            public Repository Repository;
            public string File;
            public string FilePath;
        }

    }
}