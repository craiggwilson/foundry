using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Security;

namespace Foundry.Reports
{
    public class CodeRepositoryReport : IAuthorizable<CodeRepositoryReport>
    {
        public Guid RepositoryId { get; set; }

        public Guid OwnerId { get; set; }

        public string Name { get; set; }

        public string SourceControlProvider { get; set; }

        public bool IsPrivate { get; set; }

        Guid IAuthorizable<CodeRepositoryReport>.Id
        {
            get { return RepositoryId; }
        }
    }
}