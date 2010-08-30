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
        private IUnitOfWork _domain;

        public CreateUserMessageHandler(IUnitOfWork domain)
        {
            _domain = domain;
        }

        public void Handle(CreateUserMessage message)
        {
            var user = new User(
                new Username(message.Username), 
                new Password(message.Password, message.PasswordSalt),
                message.DisplayName, 
                new Email(message.Email));

            var repo = new Repository(_domain);
            repo.Add(user);
            _domain.Commit();
        }
    }
}