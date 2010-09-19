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

        IEnumerable<Branch> GetBranches(Project project);

        IEnumerable<Commit> GetCommits(Project project, string branchName, int page, int pageCount);
    }
}