using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundry.Domain;
using Foundry.SourceControl;

namespace Foundry.Website.Models.Project
{
    public class IndexViewModel : ViewModel
    {
        public Domain.Project Project { get; set; }

        public IEnumerable<Branch> Branches { get; set; }

        public IEnumerable<Commit> Commits { get; set; }
    }
}