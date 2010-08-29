using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.Mef;
using Foundry.Reporting.Infrastructure;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Reporting.DomainEventHandlers;
using Foundry.Services;
using Foundry.Messaging.Infrastructure;
using Foundry.Reporting;
using System.ComponentModel.Composition.Hosting;

namespace Foundry.Services
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MembershipService(c.Resolve<IBus>(), c.Resolve<IReportingRepository<UserReport>>())).As<IMembershipService>().HttpRequestScoped().PropertiesAutowired();

            var catalog = new DirectoryCatalog("Addins");
            builder.RegisterComposablePartCatalog(catalog);
        }
    }
}