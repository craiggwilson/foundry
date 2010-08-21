using System.Web.Mvc;

namespace Foundry.Website.Controllers
{
    public abstract class FoundryController : Controller
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