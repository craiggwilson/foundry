using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Domain.Events.User;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Reports.Infrastructure;
using Foundry.Reports;

namespace Foundry.Domain.Events.Handlers
{
    public class UserCodeRepositoryReportEventHandler : IEventHandler<UserRepositoryAddedEvent>, IEventHandler<UserRepositoryRemovedEvent>
    {
        private readonly IReportingSession _reportingSession;


        public UserCodeRepositoryReportEventHandler(IReportingSession reportingSession)
        {
            _reportingSession = reportingSession;
        }

        public void Handle(UserRepositoryAddedEvent @event)
        {
            var userRepo = new ReportingRepository<UserReport>(_reportingSession);
            var codeRepoRepo = new ReportingRepository<CodeRepositoryReport>(_reportingSession);
            var userCodeRepoRepo = new ReportingRepository<UserCodeRepositoryReport>(_reportingSession);

            var user = userRepo.Single(x => x.UserId == @event.SourceId);
            var repo = codeRepoRepo.Single(x => x.RepositoryId == @event.RepositoryId);

            var report = new UserCodeRepositoryReport
            {
                RepositoryId = repo.RepositoryId,
                Name = repo.Name,
                SourceControlProvider = repo.SourceControlProvider,
                UserId = user.UserId,
                Username = user.Username
            };

            userCodeRepoRepo.Add(report);

            _reportingSession.Commit();
        }

        public void Handle(UserRepositoryRemovedEvent @event)
        {
            var userCodeRepoRepo = new ReportingRepository<UserCodeRepositoryReport>(_reportingSession);

            var report = userCodeRepoRepo.Single(x => x.RepositoryId == @event.RepositoryId && x.UserId == @event.SourceId);
            userCodeRepoRepo.Remove(report);

            _reportingSession.Commit();
        }
    }
}