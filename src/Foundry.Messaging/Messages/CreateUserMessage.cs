using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Messaging.Messages
{
    public class CreateUserMessage
    {
        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }
    }
}
