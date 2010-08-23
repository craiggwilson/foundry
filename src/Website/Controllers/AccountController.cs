using System.Linq;
using System.Web.Mvc;

using Foundry.Messaging;
using Foundry.Reporting;
using Foundry.Website.Models;
using Foundry.Website.Models.Account;
using Foundry.Messaging.Infrastructure;

namespace Foundry.Website.Controllers
{
    public class AccountController : FoundryController
    {
        private readonly IBus _bus;
        private readonly IReportingRepository<UserReport> _userReportRepository;

        public AccountController(IReportingRepository<UserReport> userReportRepository, IBus bus)
        {
            _userReportRepository = userReportRepository;
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
            var user = _userReportRepository.SingleOrDefault(u => u.Username == model.Username);
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
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userReportRepository.SingleOrDefault(u => u.Username == model.Username);
            if (user != null)
            {
                return View(model)
                    .WithMessage(this, "The username you have chosen is invalid.  Please try another one.", ViewMessageType.Error);
            }

            _bus.Send(new CreateUserMessage { DisplayName = model.DisplayName, Email = model.Email, Password = model.Password, Username = model.Username });

            return View("Registered");
        }
    }
}