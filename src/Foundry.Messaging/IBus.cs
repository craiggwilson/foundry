using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Messaging
{
    public interface IBus
    {

        void Send<TMessage>(TMessage message);

        TReply SendAndWaitForReply<TMessage, TReply>(TMessage message);

    }
}