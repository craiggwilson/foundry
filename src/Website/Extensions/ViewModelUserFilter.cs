using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundry.Website.Models;
using Foundry.Security;

namespace Foundry.Website.Extensions
{
    public class ViewModelUserFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewModel model;
            if (filterContext.Controller.ViewData.Model != null)
            {
                model = filterContext.Controller.ViewData.Model as ViewModel;
                if (model != null)
                    model.User = (filterContext.HttpContext.User as FoundryUser) ?? FoundryUser.Anonymous;
            }

            base.OnActionExecuted(filterContext);
        }
    }
}