using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Messages
{
    public class UserProjectCreatedMessage
    {
        public Guid ProjectId { get; set; }

        public string SourceControlProvider { get; set; }

        public string AccountName { get; set; }

        public string RepositoryName { get; set; }

        public bool IsPrivate { get; set; }

        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string UserDisplayName { get; set; }
    }
}
