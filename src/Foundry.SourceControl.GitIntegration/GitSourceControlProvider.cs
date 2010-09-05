using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;
using System.Configuration;
using GitSharp.Commands;

namespace Foundry.SourceControl.GitIntegration
{
    [SourceControlProvider("Git")]
    public class GitSourceControlProvider : ISourceControlProvider
    {
        public void CreateRepository(string name)
        {
            var cmd = new InitCommand
            {
                GitDirectory = Path.Combine(GitSettings.RepositoriesPath, name),
                Quiet = false,
                Bare = true
            };
            cmd.Execute();
        }
    }
}