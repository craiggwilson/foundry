
using Sikai.EventSourcing.Infrastructure;
using Foundry.Domain;


namespace Foundry.Reporting.DomainEventHandlers
{
    public class UserReportEventHandler : IEventHandler<User.CreatedEvent>
    {
        private readonly IReportingRepository<UserReport> _repository;

        public UserReportEventHandler(IReportingRepository<UserReport> repository)
        {
            _repository = repository;
        }

        public void Handle(User.CreatedEvent @event)
        {
            var user = new UserReport
            {
                Id = @event.SourceId,
                Username = @event.Username.Value,
                Email = @event.Email.Address,
                DisplayName = @event.DisplayName,
                Password = @event.Password.Value,
                PasswordFormat = (int)@event.Password.Format,
                Salt = @event.Password.Salt
            };

            _repository.Add(user);
        }
    }
}