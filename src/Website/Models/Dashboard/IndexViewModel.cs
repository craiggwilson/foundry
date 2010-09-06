﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundry.Reports;

namespace Foundry.Website.Models.Dashboard
{
    public class IndexViewModel : ViewModel
    {
        public IEnumerable<RepositoryReport> WritableRepositories { get; set; }
    }
}