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
    public class CodeRepositoryReportEventHandler : IEventHandler<RepositoryCreatedEvent>, IEventHandler<RepositoryDeletedEvent>
    {
        private readonly IReportingSession _reportingSession;

        public CodeRepositoryReportEventHandler(IReportingSession reportingSession)
        {
            _reportingSession = reportingSession;
        }

        public void Handle(RepositoryCreatedEvent @event)
        {
            new ReportingRepository<CodeRepositoryReport>(_reportingSession).Add(new CodeRepositoryReport
            {
                RepositoryId = @event.SourceId,
                OwnerId = @event.OwnerId,
                SourceControlProvider = @event.SourceControlProvider,
                Name = @event.Name,
                IsPrivate = @event.IsPrivate
            });

            if(!@event.IsPrivate)
            {
                var feedRepo = new ReportingRepository<NewsFeedReport>(_reportingSession);
                feedRepo.Add(new NewsFeedReport
                {
                    SubjectType = Reports.SubjectType.Repository,
                    SubjectId = @event.OwnerId, //TODO: get the current user for this property...
                    SubjectName = @event.Name,
                    DateTime = DateTime.Now,
                    Event = "Created",
                    Message = string.Format("created [[Repository: {0}]]", @event.Name)
                });
            }
            _reportingSession.Commit();
        }

        public void Handle(RepositoryDeletedEvent @event)
        {
            var repo = new ReportingRepository<CodeRepositoryReport>(_reportingSession);
            var codeRepository = repo.Single(x => x.RepositoryId == @event.SourceId);
            repo.Remove(codeRepository);

            if (!codeRepository.IsPrivate)
            {
                var feedRepo = new ReportingRepository<NewsFeedReport>(_reportingSession);
                feedRepo.Add(new NewsFeedReport
                {
                    SubjectType = Reports.SubjectType.Repository,
                    SubjectId = codeRepository.OwnerId, //TODO: get the current user for this property...
                    SubjectName = codeRepository.Name,
                    DateTime = DateTime.Now,
                    Event = "Deleted",
                    Message = string.Format("created [[Repository: {0}]]", codeRepository.Name)
                });
            }  }
    }
}