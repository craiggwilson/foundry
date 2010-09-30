using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public enum ChangeType
    {
        Added = 0,
        Deleted = 1,
        Modified = 2,
        TypeChanged = 3,
        Renamed = 4,
        Copied = 5,
    }
}