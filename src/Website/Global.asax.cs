using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.Web.Mvc;
using Spark.Web.Mvc;
using Foundry.Domain;
using Foundry.Messaging;
using Foundry.Reporting;
using System;

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
            builder.RegisterModule<ReportingeModule>();

            _containerProvider = new ContainerProvider(builder.Build());

            ControllerBuilder.Current.SetControllerFactory(new AutofacControllerFactory(ContainerProvider));

            ViewEngines.Engines.Add(new SparkViewFactory());

            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
        }
    }
}