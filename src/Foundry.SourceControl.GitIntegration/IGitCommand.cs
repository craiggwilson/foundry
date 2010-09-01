using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Foundry.SourceControl.GitIntegration
{
    public interface IGitCommand
    {
        string Name { get; }

        StreamReader Input { get; set; }

        StreamWriter Output { get; set; }

        StreamWriter Error { get; set; }

        IGitSession Session { get; }

        void Execute();
    }
}