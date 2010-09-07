using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Foundry.Security;

namespace Foundry.Website.Extensions
{
    public class FoundryBasicAuthenticationModule : IHttpModule
    {
        public void Dispose()
        { }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += Authenticate;
        }

        private void Authenticate(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;

            if (!context.Request.FilePath.EndsWith(".git"))
                return;

            var authHeader = context.Request.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Basic"))
            {
                string username, password;
                ExtractCredentials(authHeader, out username, out password);

                context.User = new FoundryUser { Id = Guid.Empty, Name = username, IsAuthenticated = true, AuthenticationType = "CustomBasic" };
                return;
            }

            context.Response.AddHeader("WWW-Authenticate", @"Basic realm=""Foundry""");
            context.Response.Write("401 Access Denied");
            context.Response.StatusCode = 401;
            context.Response.End();
        }

        private void EndRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;

            if (!context.Request.FilePath.EndsWith(".git"))
                return;

            if (context.Response.StatusCode == 401)
                context.Response.AddHeader("WWW-Authenticate", @"Basic realm=""Foundry""");
        }

        private static void ExtractCredentials(string authHeader, out string username, out string password)
        {
            var encodedUserPass = authHeader.Substring(6).Trim();

            var encoding = Encoding.GetEncoding("iso-8859-1");
            string userPass = encoding.GetString(Convert.FromBase64String(encodedUserPass));

            var seperator = userPass.IndexOf(":");
            username = userPass.Substring(0, seperator);
            password = userPass.Substring(seperator + 1);
        }


    }
}