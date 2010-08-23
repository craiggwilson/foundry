using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Foundry.Website
{
    public class FormsAuthenticationResult : ActionResult
    {
        private readonly string _username;
        private readonly bool _rememberMe;

        public FormsAuthenticationResult(string username)
            : this(username, false)
        { }

        public FormsAuthenticationResult(string username, bool rememberMe)
        {
            _username = username;
            _rememberMe = rememberMe;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            FormsAuthentication.RedirectFromLoginPage(_username, _rememberMe);
        }
    }
}