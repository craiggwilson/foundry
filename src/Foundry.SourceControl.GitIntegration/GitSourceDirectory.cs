using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitSourceDirectory : GitSourceObject, ISourceDirectory
    {
        public IEnumerable<ISourceObject> Children { get; set; }
    }
}