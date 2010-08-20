using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Foundry.Messaging.Messages
{
    public class UserAuthenticationFailedMessage
    {
        public string IpAddress { get; set; }
    }
}
