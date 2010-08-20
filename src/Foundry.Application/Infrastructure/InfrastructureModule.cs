using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Autofac;
using Autofac.Integration.Web;
using Sikai.EventSourcing.Infrastructure;
using Sikai.EventSourcing.Domain;
using Sikai.EventSourcing.Infrastructure.MsSql;
using Foundry.Messaging;

namespace Foundry.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MsSqlEventStore("", new BinaryFormatter())).As<IEventStore>();
            builder.Register(c => new AggregateBuilder()).As<IAggregateBuilder>();
            builder.Register(c => new AutofacEventHandlerFactory(c)).As<IEventHandlerFactory>();
            builder.Register(c => new Repository(c.Resolve<IUnitOfWork>())).As<IRepository>();
            builder.Register(c => new UnitOfWork(c.Resolve<IEventStore>(), c.Resolve<IAggregateBuilder>(), c.Resolve<IEventHandlerFactory>())).As<IUnitOfWork>().HttpRequestScoped();

            builder.Register(c => new InProcessBus(c)).As<IBus>().HttpRequestScoped();
        }
    }
}
