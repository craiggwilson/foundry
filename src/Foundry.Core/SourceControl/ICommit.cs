using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ICommit
    {
        string Id { get; }

        string TreeId { get; }

        string Username { get; }

        string Message { get; }

        DateTime DateTime { get; }

        IEnumerable<string> ParentIds { get; }

        IEnumerable<IChange> Changes { get; }
    }
}
