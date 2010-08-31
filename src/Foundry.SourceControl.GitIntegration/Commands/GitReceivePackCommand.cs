using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration.Commands
{
    public class GitReceivePackCommand : GitCommand
    {
        public string Directory { get; private set; }

        public override string Name
        {
            get { return "receive-pack"; }
        }

        public GitReceivePackCommand(IGitSession session, string directory)
            : base(session)
        {
            Directory = directory;
        }

        protected override IEnumerable<string> GetArguments()
        {
            yield return Quote(Directory);
        }
    }
}