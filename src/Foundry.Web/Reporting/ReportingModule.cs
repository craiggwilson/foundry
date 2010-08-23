using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Autofac;
using Autofac.Integration.Web;
using Foundry.Reporting.Infrastructure;
using Sikai.EventSourcing.Infrastructure;

namespace Foundry.Reporting
{
    public class ReportingeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ReportingDbContext())
                .As<DbContext>().As<IReportingUnitOfWork>().HttpRequestScoped();
            builder.RegisterGeneric(typeof(EfReportingRepository<>))
                .As(typeof(IReportingRepository<>));

            Database.SetInitializer(new RecreateDatabaseIfModelChanges<ReportingDbContext>());

            builder.RegisterAssemblyTypes(typeof(IReportingRepository<>).Assembly)
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
                .AsClosedTypesOf(typeof(IEventHandler<>));
        }
    }
}