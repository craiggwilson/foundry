
using System.Linq;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Domain.Events.User;
using Foundry.Reports.Infrastructure;


namespace Foundry.Reports.DomainEventHandlers
{
    public class UserReportEventHandler : IEventHandler<UserCreatedEvent>, IEventHandler<UserLoggedInEvent>
    {
        private readonly IReportingSession _reportingSession;

        public UserReportEventHandler(IReportingSession reportingSession)
        {
            _reportingSession = reportingSession;
        }

        public void Handle(UserCreatedEvent @event)
        {
            var user = new UserReport
            {
                UserId = @event.SourceId,
                Username = @event.Username,
                Email = @event.Email,
                DisplayName = @event.DisplayName,
                Password = @event.Password,
                Salt = @event.PasswordSalt,
                CreatedDateTime = @event.CreatedDateTime
            };

            new ReportingRepository<UserReport>(_reportingSession).Add(user);

            _reportingSession.Commit();
        }

        public void Handle(UserLoggedInEvent @event)
        {
            var repo = new ReportingRepository<UserReport>(_reportingSession);
            var user = repo.Single(u => u.UserId == @event.SourceId);
            user.LastLoginDateTime = @event.DateTime;

            _reportingSession.Commit();
        }
    }
}