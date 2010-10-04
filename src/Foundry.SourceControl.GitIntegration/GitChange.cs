using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitChange : IChange
    {
        public ChangeType Type { get; set; }

        public ISourceFile File { get; set; }

        public ISourceFile OldFile { get; set; }
    }
}