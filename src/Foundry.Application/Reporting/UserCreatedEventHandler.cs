using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sikai.EventSourcing.Infrastructure;

using Foundry.Domain;

namespace Foundry.Reporting
{
    public class UserCreatedEventHandler : IEventHandler<Domain.User.CreatedEvent>
    {
        public void Handle(Domain.User.CreatedEvent @event)
        {

        }
    }
}
