using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Messaging
{
    public interface IMessageHandler<TMessage>
    {
        void Handle<TMessage>(TMessage message);
    }
}
