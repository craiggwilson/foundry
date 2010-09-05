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
        private readonly IDomainSession _domainSession;

        public UserLoggedInMessageHandler(IDomainSession domainSession)
        {
            _domainSession = domainSession;
        }

        public void Handle(UserLoggedInMessage message)
        {
            var repo = new DomainRepository(_domainSession);
            var user = repo.GetById<User>(message.UserId);
            user.LoggedIn(message.DateTime);

            _domainSession.Commit();
        }
    }
}