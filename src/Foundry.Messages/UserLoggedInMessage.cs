using System;

namespace Foundry.Messaging
{
    public class UserLoggedInMessage
    {
        public Guid UserId { get; set; }

        public DateTime DateTime { get; set; }
    }
}
