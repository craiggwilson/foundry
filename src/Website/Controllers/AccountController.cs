using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Foundry.Messaging;
using Foundry.Messaging.Messages;
using Foundry.Reporting;
using Foundry.Website.Models;

namespace Foundry.Website.Controllers
{
    public class AccountController : FoundryController
    {
        private readonly IBus _bus;
        private readonly IUserReport _userReport;

        public AccountController(IUserReport userReport, IBus bus)
        {
            _userReport = userReport;
            _bus = bus;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = _userReport.FindUser(model.Username);
            if (user == null || !user.IsValidPassword(model.Password))
            {
                _bus.Send(new UserAuthenticationFailedMessage { IpAddress = ControllerContext.HttpContext.Request.ServerVariables["REMOTE_ADDR"] });
                return View(model)
                    .WithMessage(this, "The username or password provided is incorrect", ViewMessageType.Error);
            }

            if (model.RememberMe)
            {
                //TODO: do something here
            }

            //TODO: set forms auth token here...

            _bus.Send(new UserLoggedInMessage { UserId = user.Id });

            return RedirectToRoute("Default");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateUserViewModel model)
        {
            var user = _userReport.FindUser(model.Username);
            if (user != null)
            {
                return View(model);
            }

            var hashedPassword = Foundry.Reporting.User.HashPassword(Foundry.Reporting.PasswordFormat.Plain, model.Password);

            _bus.Send(new CreateUserMessage { DisplayName = model.DisplayName, Email = model.Email, Password = hashedPassword, PasswordFormat = Foundry.Messaging.Messages.PasswordFormat.Plain, Username = model.Username });

            return View("Registered");
        }
    }
}