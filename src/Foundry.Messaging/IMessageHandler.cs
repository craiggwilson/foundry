
namespace Foundry.Messaging
{
    public interface IMessageHandler<TMessage>
    {
        void Handle<TMessage>(TMessage message);
    }
}
