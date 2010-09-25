using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitSourceTree : GitSourceObject, ISourceTree
    {
        public IEnumerable<ISourceObject> Children { get; set; }
    }
}