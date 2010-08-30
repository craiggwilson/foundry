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
    public class UserLoggedInMessageHandler : IMessageHandler<UserLoggedInMessage>
    {
        private readonly IUnitOfWork _domain;

        public UserLoggedInMessageHandler(IUnitOfWork domain)
        {
            _domain = domain;
        }

        public void Handle(UserLoggedInMessage message)
        {
            var repo = new Repository(_domain);
            var user = repo.GetById<User>(message.UserId);
            user.LoggedIn();

            _domain.Commit();
        }
    }
}