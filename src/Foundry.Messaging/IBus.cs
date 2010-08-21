
namespace Foundry.Messaging
{
    public interface IBus
    {

        void Send<TMessage>(TMessage message);

        TReply SendAndWaitForReply<TMessage, TReply>(TMessage message);

    }
}