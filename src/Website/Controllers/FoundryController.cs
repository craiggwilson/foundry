using System.Web.Mvc;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Reports;
using System;
using Foundry.Security;
using Foundry.Website.Extensions;

namespace Foundry.Website.Controllers
{
    [UserFilter]
    public abstract partial class FoundryController : Controller
    {
        public const string VIEW_MESSAGE_KEY = "Message";

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