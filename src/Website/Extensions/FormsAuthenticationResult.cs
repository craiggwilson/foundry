using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Foundry.Website.Models;
using Foundry.Security;

namespace Foundry.Website
{
    public class FormsAuthenticationResult : ActionResult
    {
        private readonly FoundryUser _user;
        private readonly bool _rememberMe;

        public FormsAuthenticationResult(FoundryUser user, bool rememberMe)
        {
            _user = user;
            _rememberMe = rememberMe;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var ticket = new FormsAuthenticationTicket(1,
                _user.Name,
                DateTime.Now,
                DateTime.Now.AddDays(10),
                _rememberMe,
                _user.ToString());

            var ticketString = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketString);
            if (_rememberMe)
                cookie.Expires = DateTime.Now.AddDays(10);

            context.HttpContext.Response.Cookies.Add(cookie);

            HttpContext.Current.Response.Redirect(FormsAuthentication.GetRedirectUrl(_user.Name, false));
        }
    }
}