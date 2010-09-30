using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitCommit : GitHistoricalItem, ICommit
    {
        public IEnumerable<ISourceFile> Files { get; set; }
    }
}
