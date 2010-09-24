using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ITree
    {
        string Id { get; }

        IEnumerable<ITree> Trees { get; }

        IEnumerable<ILeaf> Leaves { get; }

        ITree Parent { get; }
    }
}
