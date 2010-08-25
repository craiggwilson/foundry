﻿using System.Linq;
using System.Web.Mvc;

using Foundry.Messaging;
using Foundry.Reporting;
using Foundry.Website.Models;
using Foundry.Website.Models.Account;
using Foundry.Messaging.Infrastructure;
using System.Web.Security;
using Foundry.Services;

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

            return new FormsAuthenticationResult(model.Username, model.RememberMe)
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