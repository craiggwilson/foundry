using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;
using System.Configuration;
using Foundry.SourceControl.GitIntegration.Commands;

namespace Foundry.SourceControl.GitIntegration
{
    [SourceControlProvider("Git")]
    public class GitSourceControlProvider : ISourceControlProvider
    {
        public void CreateRepository(string name)
        {
            using (var session = new GitSession(GitSettings.ExePath, GitSettings.RepositoriesPath))
            {
                var cmd = new GitInitCommand(session, name) { Bare = true };
                cmd.Execute();
            }
        }
    }
}