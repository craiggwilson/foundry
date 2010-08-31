using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitCommandResult
    {
        public string Error { get; private set; }
        public string Output { get; private set; }

        public GitCommandResult(string output, string error)
        {
            Output = output;
            Error = error;
        }

        public GitCommandResult ThrowIfError()
        {
            if (!string.IsNullOrWhiteSpace(Error))
                throw new Exception(Error);

            return this;
        }
    }
}