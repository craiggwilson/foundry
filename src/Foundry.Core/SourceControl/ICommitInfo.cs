using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ICommitInfo
    {
        string Username { get; }

        string Message { get; }

        DateTime DateTime { get; }

        string Version { get; }
    }
}