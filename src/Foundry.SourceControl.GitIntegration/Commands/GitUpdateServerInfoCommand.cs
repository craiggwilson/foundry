using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration.Commands
{
    public class GitUpdateServerInfoCommand : GitCommand
    {
        public bool? Force { get; set; }

        public override string Name
        {
            get { return "update-server-info"; }
        }

        public GitUpdateServerInfoCommand(IGitSession session)
            : base(session)
        { }

        protected override IEnumerable<string> GetArguments()
        {
            if (Force.GetValueOrDefault())
                yield return "--force";
        }
    }
}