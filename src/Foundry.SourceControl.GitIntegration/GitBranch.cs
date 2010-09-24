using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitBranch : IBranch
    {
        public string Name { get; set; }

        public bool IsCurrent { get; set; }
    }
}
