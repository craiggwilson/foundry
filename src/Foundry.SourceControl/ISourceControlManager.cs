using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ISourceControlManager
    {
        IEnumerable<string> ProviderNames { get; }

        ISourceControlProvider GetByName(string name);
    }
}