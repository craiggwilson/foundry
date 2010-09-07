using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ISourceControlProvider
    {
        void CreateRepository(string name);
    }
}