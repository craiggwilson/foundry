using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikai.EventSourcing.Domain;
using Foundry.Domain.Events.Repository;

namespace Foundry.Domain
{
    public class CodeRepository : AggregateRootBase
    {
        private string _sourceControlProvider;
        private string _name;

        private CodeRepository(Guid id)
            : base(id)
        {
            WireUpEventHandlers();
        }

        public CodeRepository(string sourceControlProvider, string name)
             : this(Guid.NewGuid())
        {
            Raise(new RepositoryCreatedEvent
            {
                SourceId = Id,
                SourceControlProvider = sourceControlProvider,
                Name = name
            });
        }

        private void WireUpEventHandlers()
        {
            RegisterEventHandler<RepositoryCreatedEvent>(OnCreated);
        }

        private void OnCreated(RepositoryCreatedEvent @event)
        {
            _sourceControlProvider = @event.SourceControlProvider;
            _name = @event.Name;
        }

    }
}