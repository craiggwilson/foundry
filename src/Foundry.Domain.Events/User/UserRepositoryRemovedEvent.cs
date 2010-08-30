using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sikai.EventSourcing.Domain;

namespace Foundry.Domain.Events.User
{
    [Serializable]
    public class UserRepositoryRemovedEvent : DomainEvent
    {
        public Guid RepositoryId { get; set; }
    }
}