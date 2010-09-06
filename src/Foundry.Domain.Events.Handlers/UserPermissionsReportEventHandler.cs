
using System.Linq;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Domain.Events.User;
using Foundry.Reports.Infrastructure;
using System;
using System.Collections.Generic;
using Foundry.Domain.Events.Repository;


namespace Foundry.Reports.DomainEventHandlers
{
    public class UserPermissionsReportEventHandler : IEventHandler<RepositoryCreatedEvent>, IEventHandler<RepositoryDeletedEvent>
    {
        private readonly IReportingSession _reportingSession;

        public UserPermissionsReportEventHandler(IReportingSession reportingSession)
        {
            _reportingSession = reportingSession;
        }

        public void Handle(RepositoryCreatedEvent @event)
        {
            var userAccessRepo = new ReportingRepository<UserPermissionsReport>(_reportingSession);

            userAccessRepo.Add(GetOwnerPermissions(@event.OwnerId, @event.SourceId, @event.Name));
            userAccessRepo.Add(GetEveryonePermissions(@event.SourceId, @event.Name, @event.IsPrivate));

            //TODO: when OwnerId is an organization, get all members and add their permissions here...

            _reportingSession.Commit();
        }

        public void Handle(RepositoryDeletedEvent @event)
        {
            var userAccessRepo = new ReportingRepository<UserPermissionsReport>(_reportingSession);
            var reports = userAccessRepo.Where(x => x.SubjectId == @event.SourceId);
            foreach (var access in reports)
                userAccessRepo.Remove(access);

            _reportingSession.Commit();
        }

        private static UserPermissionsReport GetOwnerPermissions(Guid userId, Guid repositoryId, string repositoryName)
        {
            return new UserPermissionsReport
            {
                UserId = userId,
                SubjectType = SubjectType.Repository,
                SubjectId = repositoryId,
                Level = 99,
                Operation = Security.Operation.All,
                Allow = true
            };
        }

        private static UserPermissionsReport GetEveryonePermissions(Guid repositoryId, string repositoryName, bool isPrivate)
        {
            return new UserPermissionsReport
            {
                UserId = Guid.Empty,
                SubjectType = SubjectType.Repository,
                SubjectId = repositoryId,
                Level = 1,
                Operation = Security.Operation.Read,
                Allow = isPrivate
            };
        }
    }
}