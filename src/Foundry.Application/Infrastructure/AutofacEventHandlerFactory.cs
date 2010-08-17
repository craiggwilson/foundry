using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Sikai.EventSourcing.Infrastructure;
using Autofac;

namespace Foundry.Infrastructure
{
    public class AutofacEventHandlerFactory : IEventHandlerFactory
    {
        private IComponentContext _componentContext;

        public AutofacEventHandlerFactory(IComponentContext context)
        {
            _componentContext = context;
        }

        public IEnumerable<IEventHandler<TDomainEvent>> ResolveHandlers<TDomainEvent>() where TDomainEvent : Sikai.EventSourcing.Domain.IDomainEvent
        {
            return _componentContext.Resolve<IEnumerable<IEventHandler<TDomainEvent>>>();
        }
    }
}
