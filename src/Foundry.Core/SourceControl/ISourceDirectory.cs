using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ISourceDirectory : ISourceObject
    {
        IEnumerable<ISourceObject> Children { get; }
    }
}