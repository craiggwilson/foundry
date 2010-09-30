using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Domain;

namespace Foundry.SourceControl
{
    public interface ISourceControlManager
    {
        IEnumerable<string> ProviderNames { get; }

        void CreateUserProject(Guid userId, string providerName, string accountName, string repositoryName, bool isPrivate);

        IEnumerable<IBranch> GetBranches(Project project);

        ICommit GetCommit(Project project, string path);

        IEnumerable<ICommitInfo> GetHistory(Project project, string path);

        ISourceControlProviderMetadata GetProviderMetadata(Project project);

        ISourceObject GetSourceObject(Project project, string path);
    }
}