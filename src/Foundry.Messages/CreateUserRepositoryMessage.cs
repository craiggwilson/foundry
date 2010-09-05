using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Messages
{
    public class CreateUserRepositoryMessage
    {
        public Guid UserId { get; set; }

        public string SourceControlProvider { get; set; }

        public string RepositoryName { get; set; }

        public bool Private { get; set; }
    }
}