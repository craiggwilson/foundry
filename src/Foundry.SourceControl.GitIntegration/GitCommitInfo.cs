using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitCommitInfo : ICommitInfo, IHasParents
    {
        public string Username { get; set; }

        public string Message { get; set; }

        public DateTime DateTime { get; set; }

        public string Version { get; set; }

        public IEnumerable<string> ParentVersions { get; set; }
    }
}
