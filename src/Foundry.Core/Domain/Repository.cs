using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Security;

namespace Foundry.Domain
{
    public class Repository : IAuthorizable<Repository>
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public string Name { get; set; }

        public string SourceControlProvider { get; set; }

        public bool IsPrivate { get; set; }
    }
}