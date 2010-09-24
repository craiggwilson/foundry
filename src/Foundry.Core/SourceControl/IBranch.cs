using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface IBranch
    {
        string Name { get; }

        bool IsCurrent { get; }
    }
}