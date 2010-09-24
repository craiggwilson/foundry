using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitTree : ITree
    {
        public string Id { get; set; }

        public IEnumerable<ITree> Trees { get; set; }

        public IEnumerable<ILeaf> Leaves { get; set; }

        public ITree Parent { get; set; }
    }
}
