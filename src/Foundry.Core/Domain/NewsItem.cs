using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Domain
{
    public class NewsItem
    {
        public string SubjectType { get; set; }

        public Guid SubjectId { get; set; }

        public string SubjectName { get; set; }

        public string Event { get; set; }

        public DateTime DateTime { get; set; }

        public string Message { get; set; }
    }

}