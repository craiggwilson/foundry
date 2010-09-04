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
            var directory = Path.Combine(GitSettings.RepositoriesPath, name);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var cmd = new InitCommand
            {
                GitDirectory = directory,
                Quiet = false,
                Bare = true
            };
            cmd.Execute();
        }
    }
}