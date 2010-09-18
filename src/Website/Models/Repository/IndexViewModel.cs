using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundry.SourceControl;

namespace Foundry.Website.Models.Repository
{
    public class IndexViewModel : ViewModel
    {
        public string AccountName { get; set; }

        public string ProjectName { get; set; }

        public IEnumerable<Commit> Commits { get; set; }

        public string RepositoryName
        {
            get { return AccountName + "/" + ProjectName; }
        }
    }
}