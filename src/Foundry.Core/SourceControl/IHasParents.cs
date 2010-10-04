using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface IHasParents
    {
        IEnumerable<string> ParentIds { get; }
    }
}
