using System.Web.Mvc;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Reports;
using System;
using Foundry.Security;

namespace Foundry.Website.Controllers
{
    public abstract partial class FoundryController : Controller
    {
        public const string VIEW_MESSAGE_KEY = "Message";

        public FoundryUser FoundryUser
        {
            get { return (FoundryUser)User; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ProcessViewMessage();
        }

        private void ProcessViewMessage()
        {
            if (TempData.ContainsKey(VIEW_MESSAGE_KEY))
                ViewData[VIEW_MESSAGE_KEY] = TempData[VIEW_MESSAGE_KEY];
        }

    }
}