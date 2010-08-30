using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Domain.Events.Repository
{
    [Serializable]
    public class RepositoryCreatedEvent : DomainEvent
    {
        public string SourceControlProvider { get; set; }

        public string Name { get; set; }
    }
}