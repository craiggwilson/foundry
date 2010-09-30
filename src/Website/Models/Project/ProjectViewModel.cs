using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundry.SourceControl;

namespace Foundry.Website.Models.Project
{
    public class ProjectViewModel : ViewModel
    {
        public IEnumerable<IBranch> Branches { get; set; }

        public IBranch DefaultBranch { get; set; }

        public IHistoricalItem LastCommit { get; set; }

        public bool IsEmpty { get; set; }

        public bool CommitsHaveParents { get; set; }

        public Domain.Project Project { get; set; }
    }
}