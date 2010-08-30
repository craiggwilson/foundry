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

        public FormsAuthenticationResult(string username)
        {
            _username = username;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpContext.Current.Response.Redirect(FormsAuthentication.GetRedirectUrl(_username, false));
        }
    }
}