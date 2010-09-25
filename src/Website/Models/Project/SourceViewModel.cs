using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundry.SourceControl;

namespace Foundry.Website.Models.Project
{
    public class SourceViewModel : ProjectViewModel
    {
        public ISourceObject Source { get; set; }
    }
}