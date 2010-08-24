using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Sikai.EventSourcing.Domain;
using Foundry.Domain;

namespace Foundry.Messaging.MessageHandlers
{
    public class UserLoggedInMessageHandler : IMessageHandler<UserLoggedInMessage>
    {
        private readonly IRepository _repository;

        public UserLoggedInMessageHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(UserLoggedInMessage message)
        {
            var user = _repository.GetById<User>(message.UserId);
            user.LoggedIn();
        }
    }
}