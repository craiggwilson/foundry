using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sikai.EventSourcing.Domain;

namespace Foundry.Domain.Events.User
{
    [Serializable]
    public class UserCreatedEvent : DomainEvent
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}