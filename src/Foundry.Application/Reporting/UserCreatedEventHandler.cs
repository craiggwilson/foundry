
using Sikai.EventSourcing.Infrastructure;


namespace Foundry.Reporting
{
    public class UserCreatedEventHandler : IEventHandler<Domain.User.CreatedEvent>
    {
        public void Handle(Domain.User.CreatedEvent @event)
        {

        }
    }
}