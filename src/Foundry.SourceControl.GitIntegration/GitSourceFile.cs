using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitSourceFile : GitSourceObject, ISourceFile
    {
        public string Content { get; set; }
    }
}