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

        private User(Guid id)
            : base(id)
        {
            WireUpEventHandlers();
        }

        public User(Username username, Password password, string displayName, Email email)
            : this(Guid.NewGuid())
        {
            Raise(new UserCreatedEvent { 
                SourceId = Id, 
                Username = username.Value, 
                Password = password.Value, 
                PasswordFormat = (int)password.Format, 
                PasswordSalt = password.Salt,
                DisplayName = displayName, 
                Email = email.Address});
        }

        private void WireUpEventHandlers()
        {
            RegisterEventHandler<UserCreatedEvent>(OnCreated);
        }

        private void OnCreated(UserCreatedEvent @event)
        {
            _displayName = @event.DisplayName;
            _username = new Username(@event.Username);
            _password = new Password(@event.Password, (PasswordFormat)@event.PasswordFormat, @event.PasswordSalt);
            _email = new Email(@event.Email);
        }
    }
}
