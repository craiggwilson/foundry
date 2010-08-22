using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Autofac;
using Autofac.Integration.Web;
using Foundry.Reporting.Infrastructure;

namespace Foundry.Reporting
{
    public class ReportingeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ReportingDbContext()).As<DbContext>().HttpRequestScoped();
            builder.RegisterGeneric(typeof(EfReportingRepository<>))
                .As(typeof(IReportingRepository<>));

            Database.SetInitializer(new RecreateDatabaseIfModelChanges<ReportingDbContext>());
        }
    }
}