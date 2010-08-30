using System;
using System.Collections.Generic;
using Sikai.EventSourcing.Domain;

using Foundry.Domain.Events.User;

namespace Foundry.Domain
{
    public class User : AggregateRootBase
    {
        private Username _username;
        private Password _password;
        private string _displayName;
        private Email _email;
        private DateTime _createdDateTime;
        private DateTime _lastLoginDateTime;

        private List<Guid> _repositories;

        private User(Guid id)
            : base(id)
        {
            WireUpEventHandlers();
            _repositories = new List<Guid>();
        }

        public User(Username username, Password password, string displayName, Email email)
            : this(Guid.NewGuid())
        {
            Raise(new UserCreatedEvent 
            { 
                SourceId = Id, 
                Username = username.Value, 
                Password = password.Value, 
                PasswordSalt = password.Salt,
                DisplayName = displayName, 
                Email = email.Address,
                CreatedDateTime = DateTime.UtcNow
            });
        }

        public void AddRepository(Guid repositoryId)
        {
            Raise(new UserRepositoryAddedEvent
            {
                SourceId = Id,
                RepositoryId = repositoryId
            });
        }

        public void RemoveRepository(Guid repositoryId)
        {
            Raise(new UserRepositoryRemovedEvent
            {
                SourceId = Id,
                RepositoryId = repositoryId
            });
        }

        public void LoggedIn()
        {
            Raise(new UserLoggedInEvent
            {
                SourceId = Id,
                DateTime = DateTime.UtcNow
            });
        }

        private void WireUpEventHandlers()
        {
            RegisterEventHandler<UserCreatedEvent>(OnCreated);
            RegisterEventHandler<UserLoggedInEvent>(OnLoggedIn);
            RegisterEventHandler<UserRepositoryAddedEvent>(OnRepositoryAdded);
            RegisterEventHandler<UserRepositoryRemovedEvent>(OnRepositoryRemoved);
        }

        private void OnCreated(UserCreatedEvent @event)
        {
            _displayName = @event.DisplayName;
            _username = new Username(@event.Username);
            _password = new Password(@event.Password, @event.PasswordSalt);
            _email = new Email(@event.Email);
            _createdDateTime = @event.CreatedDateTime;
        }

        private void OnLoggedIn(UserLoggedInEvent @event)
        {
            _lastLoginDateTime = @event.DateTime;
        }

        private void OnRepositoryAdded(UserRepositoryAddedEvent @event)
        {
            _repositories.Add(@event.RepositoryId);
        }

        private void OnRepositoryRemoved(UserRepositoryRemovedEvent @event)
        {
            _repositories.Remove(@event.RepositoryId);
        }
    }
}