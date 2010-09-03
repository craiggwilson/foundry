using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Foundry.SourceControl.GitIntegration.Commands
{
    public interface IGitWrapperCommand
    {
        string Name { get; }

        StreamReader Input { get; set; }

        StreamWriter Output { get; set; }

        StreamWriter Error { get; set; }

        void Execute();
    }
}