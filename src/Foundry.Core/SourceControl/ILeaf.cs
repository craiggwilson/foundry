using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ILeaf
    {
        string Id { get; }

        ITree Parent { get; }
    }
}
