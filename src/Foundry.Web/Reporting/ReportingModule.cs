using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Autofac;
using Autofac.Integration.Web;
using Foundry.Reports.Infrastructure;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Reports.DomainEventHandlers;

namespace Foundry.Reports
{
    public class ReportingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ReportingDbContext())
                .As<IReportingSession>();
            builder.RegisterGeneric(typeof(ReportingRepository<>))
                .As(typeof(IReportingRepository<>)).HttpRequestScoped();

            Database.SetInitializer(new RecreateDatabaseIfModelChanges<ReportingDbContext>());

            builder.RegisterAssemblyTypes(typeof(UserReportEventHandler).Assembly)
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
                .AsClosedTypesOf(typeof(IEventHandler<>));
        }
    }
}