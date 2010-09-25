using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Domain;

namespace Foundry.SourceControl
{
    public interface ISourceControlProvider
    {
        void CreateRepository(Project project);

        IEnumerable<IBranch> GetBranches(Project project);

        IEnumerable<ICommit> GetCommits(Project project, string branchName, int page, int pageCount);

        ISourceObject GetSourceObject(Project project, string id, string path);
    }
}