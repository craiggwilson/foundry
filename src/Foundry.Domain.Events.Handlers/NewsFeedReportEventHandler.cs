using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Domain.Events.Repository;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Reports.Infrastructure;
using Foundry.Reports;

namespace Foundry.Domain.Events.Handlers
{
    public class NewsFeedReportEventHandler : IEventHandler<RepositoryCreatedEvent>, IEventHandler<RepositoryDeletedEvent>
    {
        private readonly IReportingSession _reportingSession;

        public NewsFeedReportEventHandler(IReportingSession reportingSession)
        {
            _reportingSession = reportingSession;
        }

        public void Handle(RepositoryCreatedEvent @event)
        {
            if(!@event.IsPrivate)
            {
                var feedRepo = new ReportingRepository<NewsFeedReport>(_reportingSession);
                feedRepo.Add(new NewsFeedReport
                {
                    SubjectType = SubjectType.User,
                    SubjectId = @event.OwnerId, //TODO: get the current user for this property...
                    SubjectName = @event.Name,
                    DateTime = DateTime.UtcNow,
                    Event = "Repository-Created",
                    Message = string.Format("created [[Repository: {0}]]", @event.Name)
                });
            }
            _reportingSession.Commit();
        }

        public void Handle(RepositoryDeletedEvent @event)
        {
            var repo = new ReportingRepository<RepositoryReport>(_reportingSession);
            var codeRepository = repo.Single(x => x.Id == @event.SourceId);

            if (!codeRepository.IsPrivate)
            {
                var feedRepo = new ReportingRepository<NewsFeedReport>(_reportingSession);
                feedRepo.Add(new NewsFeedReport
                {
                    SubjectType = SubjectType.User,
                    SubjectId = codeRepository.OwnerId, //TODO: get the current user for this property...
                    SubjectName = codeRepository.Name,
                    DateTime = DateTime.UtcNow,
                    Event = "Repository-Deleted",
                    Message = string.Format("deleted {0}", codeRepository.Name)
                });
            }

            _reportingSession.Commit();        
        }
    }
}