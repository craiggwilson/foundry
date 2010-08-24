
using System.Linq;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Domain.Events.User;


namespace Foundry.Reporting.DomainEventHandlers
{
    public class UserReportEventHandler : IEventHandler<UserCreatedEvent>, IEventHandler<UserLoggedInEvent>
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
                Salt = @event.PasswordSalt,
                CreatedDateTime = @event.CreatedDateTime
            };

            _repository.Add(user);
        }

        public void Handle(UserLoggedInEvent @event)
        {
            var user = _repository.Single(u => u.Id == @event.SourceId);
            user.LastLoginDateTime = @event.DateTime;
        }
    }
}