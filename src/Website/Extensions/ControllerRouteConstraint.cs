using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace Foundry.Website.Extensions
{
    public class ControllerRouteConstraint : IRouteConstraint
    {
        private static readonly HashSet<string> _controllers = new HashSet<string>
        {
            "Dashboard",
            "Account",
            //"Project",
        };

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (parameterName == "controller")
                return _controllers.Contains((string)values["controller"]);

            return true;
        }
    }
}