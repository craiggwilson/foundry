using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface IHistoricalItem
    {
        string Username { get; }

        string Message { get; }

        DateTime DateTime { get; }

        string Version { get; }
    }
}