
using Sikai.EventSourcing.Infrastructure;
using Foundry.Domain.Events.User;


namespace Foundry.Reporting.DomainEventHandlers
{
    public class UserReportEventHandler : IEventHandler<UserCreatedEvent>
    {
        private readonly IReportingRepository<UserReport> _repository;

        public UserReportEventHandler(IReportingRepository<UserReport> repository)
        {
            _repository = repository;
        }

        public void Handle(UserCreatedEvent @event)
        {
            var user = new UserReport
            {
                Id = @event.SourceId,
                Username = @event.Username,
                Email = @event.Email,
                DisplayName = @event.DisplayName,
                Password = @event.Password,
                PasswordFormat = @event.PasswordFormat,
                Salt = @event.PasswordSalt
            };

            _repository.Add(user);
        }
    }
}