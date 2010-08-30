using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Reports
{
    public class UserCodeRepositoryReport
    {
        public Guid RepositoryId { get; set; }

        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string SourceControlProvider { get; set; }

        public string Name { get; set; }
    }
}