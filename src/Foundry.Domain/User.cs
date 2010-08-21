using System;
using System.Collections.Generic;
using Sikai.EventSourcing.Domain;

namespace Foundry.Domain
{
    public class User : AggregateRootBase
    {
        private Username _username;
        private string _displayName;
        private Email _email;
        private List<Guid> _codeRepositories;

        private User(Guid id)
            : base(id)
        {
            WireUpEventHandlers();
        }

        public User(Username username, string displayName, Email email)
            : this(Guid.NewGuid())
        {
            Raise(new CreatedEvent { SourceId = Id, Username = username, DisplayName = displayName, Email = email });
        }

        public void AddCodeRepository(Guid codeRepositoryId)
        {
            if (_codeRepositories.Contains(codeRepositoryId))
                throw new InvalidOperationException("The user already contains the code repository.");

            Raise(new AddCodeRepositoryEvent { SourceId = Id, CodeRepositoryId = codeRepositoryId });
        }

        public void RemoveCodeRepository(Guid codeRepositoryId)
        {
            if (!_codeRepositories.Contains(codeRepositoryId))
                throw new InvalidOperationException("The user does not contain the code repository.");

            Raise(new RemoveCodeRepositoryEvent { SourceId = Id, CodeRepositoryId = codeRepositoryId });
        }

        private void WireUpEventHandlers()
        {
            RegisterEventHandler<CreatedEvent>(OnCreated);
        }

        private void OnCreated(CreatedEvent @event)
        {
            _username = @event.Username;
            _displayName = @event.DisplayName;
            _email = @event.Email;
        }

        public class AddCodeRepositoryEvent : IDomainEvent
        {
            public Guid SourceId { get; set; }

            public Guid CodeRepositoryId { get; set; }
        }

        public class CreatedEvent : IDomainEvent
        {
            public Guid SourceId { get; set; }

            public Username Username { get; set; }

            public string DisplayName { get; set; }

            public Email Email { get; set; }
        }

        public class RemoveCodeRepositoryEvent : IDomainEvent
        {
            public Guid SourceId { get; set; }

            public Guid CodeRepositoryId { get; set; }
        }

    }
}
