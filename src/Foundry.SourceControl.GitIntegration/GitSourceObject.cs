using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitSourceObject : ISourceObject
    {
        public string Id { get; set; }

        public string Path { get; set; }

        public bool IsTree { get; set; }
    }
}