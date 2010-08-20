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
    public class AccountController : Controller
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
        public ActionResult Login(UserModel userModel)
        {
            var user = _userReport.FindUser(userModel.Username);
            if (user == null || !user.IsValidPassword(userModel.Password))
            {
                _bus.Send(new UserAuthenticationFailedMessage { IpAddress = ControllerContext.HttpContext.Request.ServerVariables["REMOTE_ADDR"] });
                return View();
            }

            if (userModel.RememberMe)
            {
                //TODO: do something here
            }

            //TODO: set forms auth token here...

            _bus.Send(new UserLoggedInMessage { UserId = user.Id });

            return RedirectToRoute(new { controller = "Dashboard" });
        }

    }
}