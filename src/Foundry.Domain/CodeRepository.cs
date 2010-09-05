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
        private bool _isDeleted;
        private Guid _ownerId;
        private bool _isPrivate;

        private CodeRepository(Guid id)
            : base(id)
        {
            WireUpEventHandlers();
        }

        public CodeRepository(Guid owner, string sourceControlProvider, string name, bool isPrivate)
             : this(Guid.NewGuid())
        {
            Raise(new RepositoryCreatedEvent
            {
                SourceId = Id,
                OwnerId = _ownerId,
                SourceControlProvider = sourceControlProvider,
                Name = name,
                IsPrivate = isPrivate
            });
        }

        public void Delete()
        {
            if (_isDeleted)
                throw new InvalidOperationException("Repository is already deleted.");

            Raise(new RepositoryDeletedEvent
            {
                SourceId = Id
            });
        }

        private void WireUpEventHandlers()
        {
            RegisterEventHandler<RepositoryCreatedEvent>(OnCreated);
            RegisterEventHandler<RepositoryDeletedEvent>(OnDeleted);
        }

        private void OnCreated(RepositoryCreatedEvent @event)
        {
            _ownerId = @event.OwnerId;
            _sourceControlProvider = @event.SourceControlProvider;
            _name = @event.Name;
            _isPrivate = @event.IsPrivate;
        }

        private void OnDeleted(RepositoryDeletedEvent @event)
        {
            _isDeleted = true;
        }

    }
}