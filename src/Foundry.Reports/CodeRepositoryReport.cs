using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Reports
{
    public class CodeRepositoryReport
    {
        public Guid RepositoryId { get; set; }

        public string SourceControlProvider { get; set; }

        public string Name { get; set; }
    }
}