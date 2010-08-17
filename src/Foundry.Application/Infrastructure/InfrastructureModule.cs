using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autofac;
using Sikai.EventSourcing.Infrastructure;
using Sikai.EventSourcing.Domain;
using Sikai.EventSourcing.Infrastructure.MsSql;
using System.Runtime.Serialization.Formatters.Binary;

namespace Foundry.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MsSqlEventStore("", new BinaryFormatter())).As<IEventStore>();
            builder.Register(c => new AggregateBuilder()).As<IAggregateBuilder>();
            builder.Register(c => new AutofacEventHandlerFactory(c)).As<IEventHandlerFactory>();
            builder.Register(c => new UnitOfWork(c.Resolve<IEventStore>(), c.Resolve<IAggregateBuilder>(), c.Resolve<IEventHandlerFactory>())).As<IUnitOfWork>();
            builder.Register(c => new Repository(c.Resolve<IUnitOfWork>())).As<IRepository>();
        }
    }
}
