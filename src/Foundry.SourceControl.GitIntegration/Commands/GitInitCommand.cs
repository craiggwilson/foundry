using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration.Commands
{
    public class GitInitCommand : GitWrapperCommand
    {
        public override string Name
        {
            get { return "init"; }
        }

        public bool? Bare { get; set; }

        public string Directory { get; private set; }

        public GitInitCommand(string directory)
        {
            Directory = directory;
        }

        protected override IEnumerable<string> GetArguments()
        {
            if (Bare.GetValueOrDefault())
                yield return "--bare";

            yield return Quote(Directory);
        }
    }
}