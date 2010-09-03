using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration.Commands
{
    public class GitUpdateServerInfoCommand : GitWrapperCommand
    {
        public bool? Force { get; set; }

        public override string Name
        {
            get { return "update-server-info"; }
        }

        public GitUpdateServerInfoCommand()
        { }

        protected override IEnumerable<string> GetArguments()
        {
            if (Force.GetValueOrDefault())
                yield return "--force";
        }
    }
}