using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;
using System.Configuration;

namespace Foundry.SourceControl.GitIntegration
{
    [SourceControlProvider("Git")]
    public class GitSourceControlProvider : ISourceControlProvider
    {
        public void CreateRepository(string name)
        {
            var cmd = new GitCommand(GitSettings.ExePath, GitSettings.RepositoriesPath);
            cmd.Execute("init --bare " + name);
        }
    }
}