using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Sikai;

namespace Foundry.Messaging.Infrastructure
{
    public class InProcessBus : IBus
    {
        private IComponentContext _context;
        private SubscriptionManager _subscriptions;

        public InProcessBus(IComponentContext context)
        {
            _context = context;
            _subscriptions = new SubscriptionManager();
        }

        public void Send<TMessage>(TMessage message)
        {
            var handlers = _context.Resolve<IEnumerable<IMessageHandler<TMessage>>>();
            Task.Factory.StartNew(() => handlers.ForEach(h => h.Handle(message)));
        }

        public TReply SendAndWaitForReply<TMessage, TReply>(TMessage message)
        {
            ManualResetEvent @event = new ManualResetEvent(false);
            TReply reply = default(TReply);
            var subscriptionId = Subscribe<TReply>(r =>
            {
                reply = r;
                @event.Set();
            });

            Send(message);

            ManualResetEvent.WaitAll(new[] { @event });

            Unsubscribe<TReply>(subscriptionId);
            return reply;
        }

        private Guid Subscribe<TMessage>(Action<TMessage> callback)
        {
            return _subscriptions.Subscribe(callback);
        }

        private void Unsubscribe<TMessage>(Guid subscriptionId)
        {
            _subscriptions.Unsubscribe<TMessage>(subscriptionId);
        }

        private class SubscriptionManager
        {
            private readonly Dictionary<Type, Dictionary<Guid, Action<object>>> _subscriptions;

            public SubscriptionManager()
            {
                _subscriptions = new Dictionary<Type, Dictionary<Guid, Action<object>>>();
            }

            public Guid Subscribe<TMessage>(Action<TMessage> messageHandler)
            {
                var messageType = typeof(TMessage);
                Dictionary<Guid, Action<object>> messageHandlers;
                if (!_subscriptions.TryGetValue(messageType, out messageHandlers))
                    _subscriptions[messageType] = messageHandlers = new Dictionary<Guid, Action<object>>();

                Guid subscriptionId = Guid.NewGuid();
                messageHandlers.Add(subscriptionId, WrapMessageHandler(messageHandler));
                return subscriptionId;
            }

            public void Publish<TMessage>(TMessage message)
            {
                var messageType = typeof(TMessage);
                Dictionary<Guid, Action<object>> messageHandlers;
                if (!_subscriptions.TryGetValue(messageType, out messageHandlers))
                    return;

                foreach (var messageHandler in messageHandlers.Values)
                    messageHandler(message);
            }

            public void Unsubscribe<TMessage>(Guid id)
            {
                var messageType = typeof(TMessage);
                Dictionary<Guid, Action<object>> messageHandlers;
                if (!_subscriptions.TryGetValue(messageType, out messageHandlers))
                    return;

                messageHandlers.Remove(id);
            }

            private static Action<object> WrapMessageHandler<TMessage>(Action<TMessage> messageHandler)
            {
                return msg => messageHandler((TMessage)msg);
            }
        }
    }
}