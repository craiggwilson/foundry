using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Domain.Events.Repository
{
    [Serializable]
    public class RepositoryCreatedEvent : DomainEvent
    {
        public Guid OwnerId { get; set; }

        public string SourceControlProvider { get; set; }

        public string Name { get; set; }

        public bool IsPrivate { get; set; }
    }
}