﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string gitPath = "";
            string reposPath = "";
            string repo = GetRepo(context.Request.RawUrl);
            repo = Path.Combine(reposPath, repo);

            if (string.IsNullOrEmpty(repo) ||
                !File.Exists(gitPath) ||
                !Directory.Exists(repo))
            {
                context.Response.StatusCode = 400;
                context.Response.End();
                return;
            }

            if (context.Request.RawUrl.IndexOf("/info/refs?service=git-receive-pack") >= 0)
            {
                GetInfoRefs(context, gitPath, repo, "receive-pack");
            }
            else if (context.Request.RawUrl.IndexOf("/git-receive-pack") >= 0 && context.Request.RequestType == "POST")
            {
                try
                {
                    ServiceRpc(context, gitPath, repo, "receive-pack");
                }
                finally
                {
                    var cmd = new GitCommand(gitPath, repo);
                    cmd.Execute("update-server-info");
                }
            }
            else if (context.Request.RawUrl.IndexOf("/info/refs?service=git-upload-pack") >= 0)
            {
                GetInfoRefs(context, gitPath, repo, "upload-pack");
            }
            else if (context.Request.RawUrl.IndexOf("/git-upload-pack") >= 0 && context.Request.RequestType == "POST")
            {
                ServiceRpc(context, gitPath, repo, "upload-pack");
            }
        }

        private static void GetInfoRefs(HttpContext context, string gitPath, string repo, string serviceName)
        {
            var fout = Path.GetTempFileName();
            context.Response.ContentType = string.Format("application/x-git-{0}-advertisement", serviceName);
            context.Response.Write(GitString("# service=git-" + serviceName));
            context.Response.Write("0000");

            var cmd = new GitCommand(gitPath, repo);
            cmd.Execute(string.Format(@"{0} --stateless-rpc --advertise-refs . > ""{1}""", serviceName, fout));

            context.Response.WriteFile(fout);
            context.Response.End();
            File.Delete(fout);
        }

        private static string GetRepo(string url)
        {
            var match = Regex.Match(url, "//(.[^\\.]+).git");
            var path = match.Success ? match.Groups[1].Value : "";
            return Path.GetFileNameWithoutExtension(path);
        }

        private static string GitString(string s)
        {
            var len = (s.Length + 4).ToString("x");
            while (len.Length < 4) 
                len = "0" + len;
            return len + s;
        }

        private static void ServiceRpc(HttpContext context, string gitPath, string repo, string serviceName)
        {
            context.Response.ContentType = string.Format("application/x-git-{0}-result", serviceName);

            var fin = Path.GetTempFileName();
            var fout = Path.GetTempFileName();

            using (var file = File.Create(fin))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = context.Request.InputStream.Read(buffer, 0, buffer.Length)) != 0)
                    file.Write(buffer, 0, bytesRead);
            }

            var cmd = new GitCommand(gitPath, repo);
            cmd.Execute(string.Format(@"{0} --stateless-rpc . < ""{1}"" > ""{2}""", serviceName, fin, fout));

            context.Response.WriteFile(fout);
            context.Response.End();
            File.Delete(fin);
            File.Delete(fout);
        }
    }
}