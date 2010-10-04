using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitHistoricalItem : IHistoricalItem, IHasParents
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Message { get; set; }

        public DateTime DateTime { get; set; }

        public IEnumerable<string> ParentIds { get; set; }
    }
}
