using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public class Commit
    {
        public string Username { get; set; }

        public string Comment { get; set; }

        public DateTime DateTime { get; set; }

        public string Version { get; set; }
    }
}