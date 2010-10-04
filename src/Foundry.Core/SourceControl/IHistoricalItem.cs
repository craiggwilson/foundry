using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface IHistoricalItem
    {
        string Id { get; }
     
        string Username { get; }

        string Message { get; }

        DateTime DateTime { get; }
    }
}