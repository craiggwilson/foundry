using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundry.Domain;

namespace Foundry.Website.Models.Dashboard
{
    public class IndexViewModel : ViewModel
    {
        public IEnumerable<Foundry.Domain.Repository> WritableRepositories { get; set; }
    }
}