using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Security;

namespace Foundry.Reports
{
    public class RepositoryReport : IAuthorizable<RepositoryReport>
    {
        public Guid RepositoryId { get; set; }

        public Guid OwnerId { get; set; }

        public string Name { get; set; }

        public string SourceControlProvider { get; set; }

        public bool IsPrivate { get; set; }

        Guid IAuthorizable<RepositoryReport>.Id
        {
            get { return RepositoryId; }
        }
    }
}