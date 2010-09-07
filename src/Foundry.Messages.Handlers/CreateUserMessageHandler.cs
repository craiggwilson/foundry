using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Foundry.Domain;

namespace Foundry.Messaging.MessageHandlers
{
    public class CreateUserMessageHandler : IMessageHandler<CreateUserMessage>
    {
        private readonly IBus _bus;
        private readonly IDomainRepository<User> _userRepository;

        public CreateUserMessageHandler(IBus bus, IDomainRepository<User> userRepository)
        {
            _userRepository = userRepository;
            _bus = bus;
        }

        public void Handle(CreateUserMessage message)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = message.Username,
                Email = message.Email,
                DisplayName = message.DisplayName,
                Password = message.Password,
                Salt = message.PasswordSalt
            };

            _userRepository.Add(user);

            _bus.Send(new UserCreatedMessage { Id = user.Id, Username = user.Username, DisplayName = user.DisplayName, Email = user.Email });
        }
    }
}