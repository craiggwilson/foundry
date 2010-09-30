using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ICommit : IHistoricalItem
    {
        IEnumerable<IChange> Changes { get; }
    }
}
