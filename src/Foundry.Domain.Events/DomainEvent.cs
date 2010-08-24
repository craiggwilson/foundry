using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikai.EventSourcing.Domain;

namespace Foundry.Domain.Events
{
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid SourceId { get; set; }
    }
}
