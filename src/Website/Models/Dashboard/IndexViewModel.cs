﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundry.Domain;

namespace Foundry.Website.Models.Dashboard
{
    public class IndexViewModel : ViewModel
    {
        public IEnumerable<NewsItem> NewsItems { get; set; }

        public IEnumerable<Domain.Project> WritableProjects { get; set; }
    }
}