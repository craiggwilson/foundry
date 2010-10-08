using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundry.Domain;
using Foundry.SourceControl;

namespace Foundry.Website.Models.Project
{
    public class CommitsViewModel : ProjectViewModel
    {
        public IEnumerable<ICommit> Commits { get; set; }
    }
}