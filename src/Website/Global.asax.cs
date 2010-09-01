using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;

using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.Web.Mvc;
using Spark.Web.Mvc;
using Foundry.Domain;
using Foundry.Messaging;
using Foundry.Reports;
using Foundry.Services;
using System;
using Spark;
using Foundry.Website.Controllers;
using System.Web.Security;
using System.Web;
using Foundry.Website.Models;
using System.Security.Principal;
using Foundry.SourceControl;

namespace Foundry.Website
{
    public class MvcApplication : System.Web.HttpApplication, IContainerProviderAccessor
    {
        private static IContainerProvider _containerProvider;

        public IContainerProvider ContainerProvider
        {
            get { return _containerProvider; }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{account}/{repo}.git/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModule<DomainModule>();
            builder.RegisterModule<MessagingModule>();
            builder.RegisterModule<ReportingModule>();
            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule<SourceControlModule>();

            _containerProvider = new ContainerProvider(builder.Build());

            ControllerBuilder.Current.SetControllerFactory(new AutofacControllerFactory(ContainerProvider));

            var batch = new SparkBatchDescriptor();
            batch.For<AccountController>()
                .For<DashboardController>();

            var viewFactory = new SparkViewFactory();
            //viewFactory.Precompile(batch);

            ViewEngines.Engines.Add(viewFactory);

            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AuthenticateRequest()
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            var cookie = HttpContext.Current.Request.Cookies[cookieName];

            if (cookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket == null || ticket.Expired)
                {
                    FormsAuthentication.RedirectToLoginPage();
                    return;
                }

                IPrincipal currentUser = null;

                try
                {
                    currentUser = FoundryUser.FromString(ticket.UserData);
                }
                catch
                {
                    FormsAuthentication.SignOut();
                    currentUser = null;
                }
                HttpContext.Current.User = currentUser;
            }
        }
    }
}