﻿
namespace Foundry.Messaging.Infrastructure
{
    public interface IMessageHandler<TMessage>
    {
        void Handle(TMessage message);
    }
}
