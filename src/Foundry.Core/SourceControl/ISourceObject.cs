using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ISourceObject
    {
        string TreeId { get; }

        string Path { get; }

        string Name { get; }

        bool IsDirectory { get; }

        DateTime DateTime { get; }

        string Message { get; }
    }
}