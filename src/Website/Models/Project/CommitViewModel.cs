using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundry.Domain;
using Foundry.SourceControl;

namespace Foundry.Website.Models.Project
{
    public class CommitViewModel : ProjectViewModel
    {
        public ICommit Commit { get; set; }
    }
}