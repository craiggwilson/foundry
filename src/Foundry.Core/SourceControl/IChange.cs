using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface IChange
    {
        ChangeType Type { get; }
    }
}
