using System;
using System.Collections.Generic;
using System.Reflection;

using Sikai.EventSourcing.Domain;
using Sikai.EventSourcing.Infrastructure;

namespace Foundry.Domain.Infrastructure
{
    public class AggregateBuilder : IAggregateBuilder
    {
        public TAggregateRoot BuildFromEventStream<TAggregateRoot>(IEnumerable<IDomainEvent> events) where TAggregateRoot : IAggregateRoot
        {
            var raise = typeof(EntityBase).GetMethod("Raise", BindingFlags.Default, null, new[] { typeof(IDomainEvent) }, null);
            dynamic root = null;
            foreach(var @event in events)
            {
                if (root == null)
                    root = (TAggregateRoot)Activator.CreateInstance(typeof(TAggregateRoot), BindingFlags.Default, null, new[] { @event.SourceId }, null);

                raise.Invoke(root, new [] { @event });
            }

            return root;
        }

        public TAggregateRoot BuildFromSnapshot<TAggregateRoot>(ISnapshot snapshot, IEnumerable<IDomainEvent> events) where TAggregateRoot :IAggregateRoot
        {
            throw new NotImplementedException();
        }
    }
}