
using System;
namespace Foundry.Messaging
{
    public class UserCreatedMessage
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }
    }
}