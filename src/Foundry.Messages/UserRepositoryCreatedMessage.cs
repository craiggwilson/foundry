using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Messages
{
    public class UserRepositoryCreatedMessage
    {
        public Guid RepositoryId { get; set; }

        public string SourceControlProvider { get; set; }

        public string AccountName { get; set; }

        public string ProjectName { get; set; }

        public bool IsPrivate { get; set; }

        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string UserDisplayName { get; set; }
    }
}
