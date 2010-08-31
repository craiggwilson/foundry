using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public interface IGitSession : IDisposable
    {
        string Path { get; }

        string WorkingDirectory { get; }
    }
}