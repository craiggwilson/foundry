using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Sikai.EventSourcing.Domain;
using Foundry.Domain;

namespace Foundry.Messaging.MessageHandlers
{
    public class CreateUserMessageHandler : IMessageHandler<CreateUserMessage>
    {
        private readonly IRepository _repository;

        public CreateUserMessageHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(CreateUserMessage message)
        {
            var user = new User(
                new Username(message.Username), 
                new Password(message.Password, message.PasswordSalt),
                message.DisplayName, 
                new Email(message.Email));

            _repository.Add(user);
        }
    }
}