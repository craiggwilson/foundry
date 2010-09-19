using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Security;

namespace Foundry.Domain
{
    public class NewsItem
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string UserDisplayName { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectFullName { get; set; }

        public string Event { get; set; }

        public DateTime DateTime { get; set; }

        public string Message { get; set; }
    }
}