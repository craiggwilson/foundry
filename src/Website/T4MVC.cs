﻿// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
public static class MVC {
    public static Foundry.Website.Controllers.AccountController Account = new Foundry.Website.Controllers.T4MVC_AccountController();
    public static Foundry.Website.Controllers.DashboardController Dashboard = new Foundry.Website.Controllers.T4MVC_DashboardController();
    public static Foundry.Website.Controllers.ProjectController Project = new Foundry.Website.Controllers.T4MVC_ProjectController();
    public static T4MVC.LayoutsController Layouts = new T4MVC.LayoutsController();
    public static T4MVC.SharedController Shared = new T4MVC.SharedController();
}

namespace T4MVC {
}

namespace System.Web.Mvc {
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static class T4Extensions {
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, ActionResult result) {
            return htmlHelper.RouteLink(linkText, result.GetRouteValueDictionary());
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, ActionResult result, object htmlAttributes) {
            return ActionLink(htmlHelper, linkText, result, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, ActionResult result, IDictionary<string, object> htmlAttributes) {
            return htmlHelper.RouteLink(linkText, result.GetRouteValueDictionary(), htmlAttributes);
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, ActionResult result, FormMethod formMethod) {
            return htmlHelper.BeginForm(result, formMethod, null);
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, ActionResult result, FormMethod formMethod, object htmlAttributes) {
            return BeginForm(htmlHelper, result, formMethod, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, ActionResult result, FormMethod formMethod, IDictionary<string, object> htmlAttributes) {
            var callInfo = result.GetT4MVCResult();
            return htmlHelper.BeginForm(callInfo.Action, callInfo.Controller, callInfo.RouteValueDictionary, formMethod, htmlAttributes);
        }

        public static void RenderAction(this HtmlHelper htmlHelper, ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            htmlHelper.RenderAction(callInfo.Action, callInfo.Controller, callInfo.RouteValueDictionary);
        }

        public static MvcHtmlString Action(this HtmlHelper htmlHelper, ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return htmlHelper.Action(callInfo.Action, callInfo.Controller, callInfo.RouteValueDictionary);
        }
        public static string Action(this UrlHelper urlHelper, ActionResult result) {
            return urlHelper.RouteUrl(result.GetRouteValueDictionary());
        }

        public static string ActionAbsolute(this UrlHelper urlHelper, ActionResult result) {
            return string.Format("{0}{1}",urlHelper.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority),
                urlHelper.RouteUrl(result.GetRouteValueDictionary()));
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, string linkText, ActionResult result, AjaxOptions ajaxOptions) {
            return ajaxHelper.RouteLink(linkText, result.GetRouteValueDictionary(), ajaxOptions);
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, string linkText, ActionResult result, AjaxOptions ajaxOptions, object htmlAttributes) {
            return ajaxHelper.RouteLink(linkText, result.GetRouteValueDictionary(), ajaxOptions, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, string linkText, ActionResult result, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            return ajaxHelper.RouteLink(linkText, result.GetRouteValueDictionary(), ajaxOptions, htmlAttributes);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, ActionResult result) {
            return MapRoute(routes, name, url, result, null /*namespaces*/);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, ActionResult result, object defaults) {
            return MapRoute(routes, name, url, result, defaults, null /*constraints*/, null /*namespaces*/);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, ActionResult result, string[] namespaces) {
            return MapRoute(routes, name, url, result, null /*defaults*/, namespaces);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, ActionResult result, object defaults, object constraints) {
            return MapRoute(routes, name, url, result, defaults, constraints, null /*namespaces*/);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, ActionResult result, object defaults, string[] namespaces) {
            return MapRoute(routes, name, url, result, defaults, null /*constraints*/, namespaces);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, ActionResult result, object defaults, object constraints, string[] namespaces) {
            // Create and add the route
            var route = CreateRoute(url, result, defaults, constraints, namespaces);
            routes.Add(name, route);
            return route;
        }

        // Note: can't name the AreaRegistrationContext methods 'MapRoute', as that conflicts with the existing methods
        public static Route MapRouteArea(this AreaRegistrationContext context, string name, string url, ActionResult result) {
            return MapRouteArea(context, name, url, result, null /*namespaces*/);
        }

        public static Route MapRouteArea(this AreaRegistrationContext context, string name, string url, ActionResult result, object defaults) {
            return MapRouteArea(context, name, url, result, defaults, null /*constraints*/, null /*namespaces*/);
        }

        public static Route MapRouteArea(this AreaRegistrationContext context, string name, string url, ActionResult result, string[] namespaces) {
            return MapRouteArea(context, name, url, result, null /*defaults*/, namespaces);
        }

        public static Route MapRouteArea(this AreaRegistrationContext context, string name, string url, ActionResult result, object defaults, object constraints) {
            return MapRouteArea(context, name, url, result, defaults, constraints, null /*namespaces*/);
        }

        public static Route MapRouteArea(this AreaRegistrationContext context, string name, string url, ActionResult result, object defaults, string[] namespaces) {
            return MapRouteArea(context, name, url, result, defaults, null /*constraints*/, namespaces);
        }

        public static Route MapRouteArea(this AreaRegistrationContext context, string name, string url, ActionResult result, object defaults, object constraints, string[] namespaces) {
            // Create and add the route
            var route = CreateRoute(url, result, defaults, constraints, namespaces);
            context.Routes.Add(name, route);
            route.DataTokens["area"] = context.AreaName;
            return route;
        }

        private static Route CreateRoute(string url, ActionResult result, object defaults, object constraints, string[] namespaces) {
            // Start by adding the default values from the anonymous object (if any)
            var routeValues = new RouteValueDictionary(defaults);

            // Then add the Controller/Action names and the parameters from the call
            foreach (var pair in result.GetRouteValueDictionary()) {
                routeValues.Add(pair.Key, pair.Value);
            }

            var routeConstraints = new RouteValueDictionary(constraints);

            // Create and add the route
            var route = new Route(url, routeValues, routeConstraints, new MvcRouteHandler());

            route.DataTokens = new RouteValueDictionary();

            if (namespaces != null && namespaces.Length > 0) {
                route.DataTokens["Namespaces"] = namespaces;
            }

            return route;
        }

        public static IT4MVCActionResult GetT4MVCResult(this ActionResult result) {
            var t4MVCResult = result as IT4MVCActionResult;
            if (t4MVCResult == null) {
                throw new InvalidOperationException("T4MVC methods can only be passed pseudo-action calls (e.g. MVC.Home.About()), and not real action calls.");
            }
            return t4MVCResult;
        }

        public static RouteValueDictionary GetRouteValueDictionary(this ActionResult result) {
            return result.GetT4MVCResult().RouteValueDictionary;
        }

        public static ActionResult AddRouteValues(this ActionResult result, object routeValues) {
            return result.AddRouteValues(new RouteValueDictionary(routeValues));
        }

        public static ActionResult AddRouteValues(this ActionResult result, RouteValueDictionary routeValues) {
            RouteValueDictionary currentRouteValues = result.GetRouteValueDictionary();

            // Add all the extra values
            foreach (var pair in routeValues) {
                currentRouteValues.Add(pair.Key, pair.Value);
            }

            return result;
        }

        public static ActionResult AddRouteValues(this ActionResult result, System.Collections.Specialized.NameValueCollection nameValueCollection) {
            // Copy all the values from the NameValueCollection into the route dictionary
            nameValueCollection.CopyTo(result.GetRouteValueDictionary());
            return result;
        }

        public static ActionResult AddRouteValue(this ActionResult result, string name, object value) {
            RouteValueDictionary routeValues = result.GetRouteValueDictionary();
            routeValues.Add(name, value);
            return result;
        }
        
        public static void InitMVCT4Result(this IT4MVCActionResult result, string area, string controller, string action) {
            result.Controller = controller;
            result.Action = action;
            result.RouteValueDictionary = new RouteValueDictionary();
             
            result.RouteValueDictionary.Add("Controller", controller);
            result.RouteValueDictionary.Add("Action", action);
        }

        public static bool FileExists(string virtualPath) {
            if (!HostingEnvironment.IsHosted) return false;
            string filePath = HostingEnvironment.MapPath(virtualPath);
            return System.IO.File.Exists(filePath);
        }

        static DateTime CenturyBegin=new DateTime(2001,1,1);
        public static string TimestampString(string virtualPath) {
            if (!HostingEnvironment.IsHosted) return string.Empty;
            string filePath = HostingEnvironment.MapPath(virtualPath);
            return Convert.ToString((System.IO.File.GetLastWriteTimeUtc(filePath).Ticks-CenturyBegin.Ticks)/1000000000,16);            
        }
    }
}

   
[GeneratedCode("T4MVC", "2.0")]   
public interface IT4MVCActionResult {   
    string Action { get; set; }   
    string Controller { get; set; }   
    RouteValueDictionary RouteValueDictionary { get; set; }   
}   
  

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
public class T4MVC_ActionResult : System.Web.Mvc.ActionResult, IT4MVCActionResult {
    public T4MVC_ActionResult(string area, string controller, string action): base()  {
        this.InitMVCT4Result(area, controller, action);
    }
     
    public override void ExecuteResult(System.Web.Mvc.ControllerContext context) { }
    
    public string Controller { get; set; }
    public string Action { get; set; }
    public RouteValueDictionary RouteValueDictionary { get; set; }
}



namespace Links {
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static class Scripts {
        private const string URLPATH = "~/Scripts";
        public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
        public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
        public static readonly string prettify_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/prettify.min.js") ? Url("prettify.min.js") : Url("prettify.js");
                      
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static class Content {
        private const string URLPATH = "~/Content";
        public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
        public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
        public static readonly string Application_less = Url("Application.less");
        public static readonly string prettify_css = Url("prettify.css");
    }

}

static class T4MVCHelpers {
    // You can change the ProcessVirtualPath method to modify the path that gets returned to the client.
    // e.g. you can prepend a domain, or append a query string:
    //      return "http://localhost" + path + "?foo=bar";
    private static string ProcessVirtualPathDefault(string virtualPath) {
        // The path that comes in starts with ~/ and must first be made absolute
        string path = VirtualPathUtility.ToAbsolute(virtualPath);
        
        // Add your own modifications here before returning the path
        return path;
    }

    // Calling ProcessVirtualPath through delegate to allow it to be replaced for unit testing
    public static Func<string, string> ProcessVirtualPath = ProcessVirtualPathDefault;


    // Logic to determine if the app is running in production or dev environment
    public static bool IsProduction() { 
        return (HttpContext.Current != null && !HttpContext.Current.IsDebuggingEnabled); 
    }
}


namespace Foundry.Website.Controllers {
    public partial class AccountController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected AccountController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }


        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AccountController Actions { get { return MVC.Account; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Account";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Index = "Index";
            public readonly string Login = "Login";
            public readonly string Logout = "Logout";
            public readonly string Register = "Register";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string Index = "~/Views/Account/Index.spark";
            public readonly string Login = "~/Views/Account/Login.spark";
            public readonly string Register = "~/Views/Account/Register.spark";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_AccountController: Foundry.Website.Controllers.AccountController {
        public T4MVC_AccountController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Index() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Index);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Login() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Login);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Login(Foundry.Website.Models.Account.LoginViewModel model) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Login);
            callInfo.RouteValueDictionary.Add("model", model);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Logout() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Logout);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Register() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Register);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Register(Foundry.Website.Models.Account.RegisterViewModel model) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Register);
            callInfo.RouteValueDictionary.Add("model", model);
            return callInfo;
        }

    }
}

namespace Foundry.Website.Controllers {
    public partial class DashboardController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected DashboardController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Index() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Index);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Yours() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Yours);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public DashboardController Actions { get { return MVC.Dashboard; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Dashboard";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Index = "Index";
            public readonly string Yours = "Yours";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string Index = "~/Views/Dashboard/Index.spark";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_DashboardController: Foundry.Website.Controllers.DashboardController {
        public T4MVC_DashboardController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Index(Foundry.Security.FoundryUser user) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Index);
            callInfo.RouteValueDictionary.Add("user", user);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Yours(Foundry.Security.FoundryUser user) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Yours);
            callInfo.RouteValueDictionary.Add("user", user);
            return callInfo;
        }

    }
}

namespace Foundry.Website.Controllers {
    public partial class ProjectController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ProjectController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Index() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Index);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Source() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Source);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ProjectController Actions { get { return MVC.Project; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Project";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Create = "Create";
            public readonly string Index = "Index";
            public readonly string Source = "Source";
        }


        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string Create = "~/Views/Project/Create.spark";
            public readonly string Index = "~/Views/Project/Index.spark";
            public readonly string Source = "~/Views/Project/Source.spark";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_ProjectController: Foundry.Website.Controllers.ProjectController {
        public T4MVC_ProjectController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Create() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Create);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Create(Foundry.Website.Models.Project.CreateViewModel model, Foundry.Security.FoundryUser user) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Create);
            callInfo.RouteValueDictionary.Add("model", model);
            callInfo.RouteValueDictionary.Add("user", user);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Index(string account, string repository) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Index);
            callInfo.RouteValueDictionary.Add("account", account);
            callInfo.RouteValueDictionary.Add("repository", repository);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Source(string account, string repository, string path) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Source);
            callInfo.RouteValueDictionary.Add("account", account);
            callInfo.RouteValueDictionary.Add("repository", repository);
            callInfo.RouteValueDictionary.Add("path", path);
            return callInfo;
        }

    }
}

namespace T4MVC {
    public class LayoutsController {

        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string Application = "~/Views/Layouts/Application.spark";
        }
    }

}

namespace T4MVC {
    public class SharedController {

        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string _global = "~/Views/Shared/_global.spark";
            public readonly string _Project = "~/Views/Shared/_Project.spark";
            public readonly string _User = "~/Views/Shared/_User.spark";
            public readonly string _ViewMessage = "~/Views/Shared/_ViewMessage.spark";
        }
    }

}



namespace T4MVC {
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class Dummy {
        private Dummy() { }
        public static Dummy Instance = new Dummy();
    }
}

	
		
namespace System.Web.Mvc {
    [GeneratedCode("T4MVC", "2.0")]
    public static class HtmlHelpersForExplicitPartials {
    	 
    }
	
}
	

#endregion T4MVC
#pragma warning restore 1591


