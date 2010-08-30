using System.Linq;
using System.Web.Mvc;

using Foundry.Messaging;
using Foundry.Reports;
using Foundry.Website.Models;
using Foundry.Website.Models.Account;
using Foundry.Messaging.Infrastructure;
using System.Web.Security;
using Foundry.Services;
using System;
using System.Web;

namespace Foundry.Website.Controllers
{
    public partial class AccountController : FoundryController
    {
        private readonly IMembershipService _membershipService;

        public AccountController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [HttpGet]
        public virtual ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _membershipService.TryLogin(model.Username, model.Password);
            if (!result.Item1)
            {
                return View(model)
                    .WithMessage(this, "The username or password provided is incorrect", ViewMessageType.Error);
            }

            var foundryIdentity = new FoundryUser { Id = result.Item2.UserId, Name = result.Item2.Username, DisplayName = result.Item2.DisplayName, IsAuthenticated = true, AuthenticationType = "Forms" };
            
            var ticket = new FormsAuthenticationTicket(1,
                model.Username,
                DateTime.Now,
                DateTime.Now.AddDays(10),
                model.RememberMe,
                foundryIdentity.ToString());


            var ticketString = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketString);
            if (model.RememberMe)
                cookie.Expires = DateTime.Now.AddDays(10);

            Response.Cookies.Add(cookie);

            return new FormsAuthenticationResult(model.Username)
                .WithMessage(this, string.Format("Welcome back, {0}", result.Item2.DisplayName), ViewMessageType.Info);
        }

        public virtual ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction(MVC.Account.Login());
        }

        [HttpGet]
        public virtual ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!_membershipService.CreateUser(model.Username, model.Password, model.DisplayName, model.Email))
            {
                return View(model)
                    .WithMessage(this, "The username you have chosen is invalid.  Please try another one.", ViewMessageType.Error);
            }

            return RedirectToAction(MVC.Account.Login())
                .WithMessage(this, "Your user has been created. Please login to confirm.", ViewMessageType.Info);
        }
    }
}