using System.Configuration;
using Autofac;
using Autofac.Integration.Web;
using System.Data.Entity.Infrastructure;
using Foundry.Domain.Infrastructure;

namespace Foundry.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FoundryDbContext>().As<IDomainSession>().HttpRequestScoped();
            builder.RegisterGeneric(typeof(FoundryRepository<>))
                .As(typeof(IDomainRepository<>)).HttpRequestScoped();

            Database.SetInitializer(new RecreateDatabaseIfModelChanges<FoundryDbContext>());
        }
    }
}