using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitSourceObject : ISourceObject
    {
        public string CommitId { get; set; }

        public string TreeId { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public bool IsDirectory { get; set; }

        public DateTime DateTime { get; set; }

        public string Message { get; set; }
    }
}