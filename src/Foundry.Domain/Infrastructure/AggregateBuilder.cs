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
            object root = null;
            foreach(var @event in events)
            {
                if (root == null)
                    root = GetConstructorInfo(typeof(TAggregateRoot)).Invoke(new object[] { @event.SourceId });

                raise.MakeGenericMethod(@event.GetType()).Invoke(root, new [] { @event });
            }

            return (TAggregateRoot)root;
        }

        public TAggregateRoot BuildFromSnapshot<TAggregateRoot>(ISnapshot snapshot, IEnumerable<IDomainEvent> events) where TAggregateRoot :IAggregateRoot
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