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
    public class RepositoryReportEventHandler : IEventHandler<RepositoryCreatedEvent>, IEventHandler<RepositoryDeletedEvent>
    {
        private readonly IReportingSession _reportingSession;

        public RepositoryReportEventHandler(IReportingSession reportingSession)
        {
            _reportingSession = reportingSession;
        }

        public void Handle(RepositoryCreatedEvent @event)
        {
            new ReportingRepository<RepositoryReport>(_reportingSession).Add(new RepositoryReport
            {
                RepositoryId = @event.SourceId,
                OwnerId = @event.OwnerId,
                SourceControlProvider = @event.SourceControlProvider,
                Name = @event.Name,
                IsPrivate = @event.IsPrivate
            });
            _reportingSession.Commit();
        }

        public void Handle(RepositoryDeletedEvent @event)
        {
            var repo = new ReportingRepository<RepositoryReport>(_reportingSession);
            var codeRepository = repo.Single(x => x.RepositoryId == @event.SourceId);
            repo.Remove(codeRepository);

            _reportingSession.Commit();
        }
    }
}