using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration.Commands
{
    public class RawGitCommand : GitWrapperCommand
    {
        private readonly string _name;
        private readonly List<string> _arguments;

        public override string Name
        {
            get { return _name; }
        }

        public ICollection<string> Arguments
        {
            get { return _arguments; }
        }

        public RawGitCommand(string name)
        {
            _name = name;
            _arguments = new List<string>();
        }

        protected override IEnumerable<string> GetArguments()
        {
            return _arguments;
        }
    }
}
