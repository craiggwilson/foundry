using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitCommit : GitCommitInfo, ICommit
    {
        public IEnumerable<ISourceFile> Files { get; set; }
    }
}
