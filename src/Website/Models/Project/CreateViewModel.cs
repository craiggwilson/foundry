using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Foundry.Website.Models.Project
{
    public class CreateViewModel : ViewModel
    {
        public string Name { get; set; }

        public string SelectedProviderName { get; set; }

        public IEnumerable<SelectListItem> ProviderNames { get; set; }
    }
}