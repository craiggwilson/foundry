using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitLeaf : ILeaf
    {
        public string Id { get; set; }

        public ITree Parent { get; set; }
    }
}
