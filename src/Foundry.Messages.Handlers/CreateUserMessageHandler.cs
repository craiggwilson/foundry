using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Sikai.EventSourcing.Domain;
using Foundry.Domain;
using Sikai.EventSourcing.Infrastructure;

namespace Foundry.Messaging.MessageHandlers
{
    public class CreateUserMessageHandler : IMessageHandler<CreateUserMessage>
    {
        private IDomainSession _domainSession;

        public CreateUserMessageHandler(IDomainSession domainSession)
        {
            _domainSession = domainSession;
        }

        public void Handle(CreateUserMessage message)
        {
            var user = new User(
                new Username(message.Username), 
                new Password(message.Password, message.PasswordSalt),
                message.DisplayName, 
                new Email(message.Email));

            var repo = new DomainRepository(_domainSession);
            repo.Add(user);
            _domainSession.Commit();
        }
    }
}