using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ISourceObject
    {
        string Name { get; }

        bool IsTree { get; }

        string Path { get; }

        DateTime LastModified { get; }

        string Message { get; }
    }
}