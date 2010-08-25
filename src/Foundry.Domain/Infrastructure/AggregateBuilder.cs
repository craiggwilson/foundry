using System;
using System.Collections.Generic;
using System.Reflection;

using Sikai.EventSourcing.Domain;
using Sikai.EventSourcing.Infrastructure;

namespace Foundry.Domain.Infrastructure
{
    public class AggregateBuilder : IAggregateBuilder
    {
        private readonly static Dictionary<Type, ConstructorInfo> _constructors = new Dictionary<Type, ConstructorInfo>();

        public TAggregateRoot BuildFromEventStream<TAggregateRoot>(IEnumerable<IDomainEvent> events) where TAggregateRoot : IAggregateRoot
        {
            var raise = typeof(EntityBase).GetMethod("Raise", BindingFlags.NonPublic | BindingFlags.Instance);
            TAggregateRoot root = default(TAggregateRoot);
            foreach(var @event in events)
            {
                if (root == null)
                    root = (TAggregateRoot)GetConstructorInfo(typeof(TAggregateRoot)).Invoke(new object[] { @event.SourceId });

                raise.MakeGenericMethod(@event.GetType()).Invoke(root, new [] { @event });
            }

            root.FlushEvents(); //get rid of these so we don't raise them again...
            return root;
        }

        public TAggregateRoot BuildFromSnapshot<TAggregateRoot>(ISnapshot snapshot, IEnumerable<IDomainEvent> events) where TAggregateRoot : IAggregateRoot
        {
            throw new NotImplementedException();
        }

        private static ConstructorInfo GetConstructorInfo(Type type)
        {
            ConstructorInfo ctor;
            if(!_constructors.TryGetValue(type, out ctor))
                _constructors[type] = ctor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Guid) }, null);
            return ctor;
        }
    }
}