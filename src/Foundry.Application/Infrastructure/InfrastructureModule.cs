using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Runtime.Serialization.Formatters.Binary;
using Autofac;
using Autofac.Integration.Web;
using Foundry.Messaging;
using Foundry.Reporting;
using Sikai.EventSourcing.Domain;
using Sikai.EventSourcing.Infrastructure;
using Sikai.EventSourcing.Infrastructure.Sqlite;

namespace Foundry.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new SqliteEventStore(new SQLiteConnection(ConfigurationManager.ConnectionStrings["EventStore"].ConnectionString), new BinaryFormatter())).As<IEventStore>();
            builder.Register(c => new AggregateBuilder()).As<IAggregateBuilder>();
            builder.Register(c => new AutofacEventHandlerFactory(c)).As<IEventHandlerFactory>();
            builder.Register(c => new Repository(c.Resolve<IUnitOfWork>())).As<IRepository>();
            builder.Register(c => new UnitOfWork(c.Resolve<IEventStore>(), c.Resolve<IAggregateBuilder>(), c.Resolve<IEventHandlerFactory>())).As<IUnitOfWork>().HttpRequestScoped();

            builder.Register(c => new InProcessBus(c)).As<IBus>().HttpRequestScoped();

            builder.Register(c => new ReportingDbContext()).As<DbContext>().HttpRequestScoped();
            builder.RegisterGeneric(typeof(EfReportingRepository<>))
                .As(typeof(IReportingRepository<>));

            Database.SetInitializer(new RecreateDatabaseIfModelChanges<ReportingDbContext>());
        }
    }
}