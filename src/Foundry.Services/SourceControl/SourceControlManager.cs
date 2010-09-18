﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Foundry.SourceControl;
using Foundry.Messaging.Infrastructure;
using Foundry.Messages;

namespace Foundry.Services.SourceControl
{
    [Export]
    public class SourceControlManager : ServiceBase, ISourceControlManager
    {
        private readonly IBus _bus;
        private readonly IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>> _sourceControlProviders;

        public IEnumerable<string> ProviderNames
        {
            get { return _sourceControlProviders.Select(x => x.Metadata.Name); }
        }

        public SourceControlManager(IBus bus, IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>> sourceControlProviders)
        {
            _bus = bus;
            _sourceControlProviders = sourceControlProviders;
        }

        public void CreateUserRepository(Guid userId, string providerName, string accountName, string projectName)
        {
            _bus.Send(new CreateUserRepositoryMessage { UserId = userId, SourceControlProvider = providerName, AccountName = accountName, ProjectName = projectName });
        }

        public IEnumerable<Commit> GetCommits(string providerName, string accountName, string projectName, int page, int pageCount)
        {
            var provider = _sourceControlProviders.Single(x => x.Metadata.Name == providerName);
            return provider.Value.GetCommits(accountName + "/" + projectName, page, pageCount);
        }
    }
}