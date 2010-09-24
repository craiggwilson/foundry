using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundry.SourceControl;

namespace Foundry.Website.Models.Project
{
    public class TreeViewModel : ProjectViewModel
    {
        public ITree Tree { get; set; }
    }
}