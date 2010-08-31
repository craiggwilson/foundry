using System.Configuration;
using System.Data.SQLite;
using System.Runtime.Serialization.Formatters.Binary;
using Autofac;
using Autofac.Integration.Web;
using Foundry.Domain.Infrastructure;
using Sikai.EventSourcing.Domain;
using Sikai.EventSourcing.Infrastructure;
using Sikai.EventSourcing.Infrastructure.Sqlite;

namespace Foundry.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var connString = ConfigurationManager.ConnectionStrings["EventStore"].ConnectionString;

            using (var conn = new SQLiteConnection(connString))
            {
                conn.Open();
                SchemaGenerator.EnsureSchemaExists(conn);
            }

            builder.Register(c => new SqliteEventStore(new SQLiteConnection(connString), new XmlFormatter())).As<IEventStore>();
            builder.Register(c => new AggregateBuilder()).As<IAggregateBuilder>();
            builder.Register(c => new AutofacEventHandlerFactory(c)).As<IEventHandlerFactory>();
            builder.Register(c => new DomainSession(c.Resolve<IEventStore>(), c.Resolve<IAggregateBuilder>(), c.Resolve<IEventHandlerFactory>())).As<IDomainSession>();
        }
    }
}