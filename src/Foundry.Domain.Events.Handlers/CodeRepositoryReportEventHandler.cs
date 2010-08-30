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
            var repo = new CodeRepositoryReport
            {
                RepositoryId = @event.SourceId,
                SourceControlProvider = @event.SourceControlProvider,
                Name = @event.Name
            };

            new ReportingRepository<CodeRepositoryReport>(_reportingSession).Add(repo);

            _reportingSession.Commit();
        }

        public void Handle(RepositoryDeletedEvent @event)
        {
            var repo = new ReportingRepository<CodeRepositoryReport>(_reportingSession);
            var codeRepository = repo.Single(x => x.RepositoryId == @event.SourceId);
            repo.Remove(codeRepository);

            _reportingSession.Commit();
        }
    }
}