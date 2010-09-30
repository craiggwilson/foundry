using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Foundry.SourceControl;
using Foundry.Messaging.Infrastructure;
using Foundry.Messages;
using Foundry.Domain;

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

        public ISourceControlProviderMetadata GetProviderMetadata(Project project)
        {
            return _sourceControlProviders.Single(x => x.Metadata.Name == project.SourceControlProvider).Metadata;
        }

        public void CreateUserProject(Guid userId, string providerName, string accountName, string repositoryName, bool isPrivate)
        {
            _bus.Send(new CreateUserProjectMessage { UserId = userId, SourceControlProvider = providerName, AccountName = accountName, RepositoryName = repositoryName, IsPrivate = isPrivate });
        }

        public IEnumerable<IBranch> GetBranches(Project project)
        {
            var provider = _sourceControlProviders.Single(x => x.Metadata.Name == project.SourceControlProvider);
            return provider.Value.GetBranches(project);
        }

        public ICommit GetCommit(Project project, string path)
        {
            var provider = _sourceControlProviders.Single(x => x.Metadata.Name == project.SourceControlProvider);
            return provider.Value.GetCommit(project, path);
        }

        public IEnumerable<IHistoricalItem> GetHistory(Project project, string path)
        {
            var provider = _sourceControlProviders.Single(x => x.Metadata.Name == project.SourceControlProvider);
            return provider.Value.GetHistory(project, path);
        }

        public ISourceObject GetSourceObject(Project project, string path)
        {
            var provider = _sourceControlProviders.Single(x => x.Metadata.Name == project.SourceControlProvider);
            return provider.Value.GetSourceObject(project, path);
        }
    }
}