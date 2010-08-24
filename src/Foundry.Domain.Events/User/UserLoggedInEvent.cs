using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Domain.Events.User
{
    [Serializable]
    public class UserLoggedInEvent : DomainEvent
    {
        public DateTime DateTime { get; set; }
    }
}
