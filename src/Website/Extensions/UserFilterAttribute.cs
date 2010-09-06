using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundry.Security;

namespace Foundry.Website.Extensions
{
    public class UserFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            const string key = "user";

            if (filterContext.ActionParameters.ContainsKey(key))
            {
                var user = filterContext.HttpContext.User as FoundryUser;
                if (user != null)
                {
                    filterContext.ActionParameters[key] = user;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}