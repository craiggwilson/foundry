using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public interface IGitCommand
    {
        string Name { get; }

        string Infile { get; set; }

        string Outfile { get; set; }

        IGitSession Session { get; }

        GitCommandResult Execute();
    }
}
