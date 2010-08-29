using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;

namespace Foundry.SourceControl.GitIntegration
{
    [SourceControlProvider("Git")]
    public class GitSourceControlProvider
    {
        private readonly string _gitPath;
        private readonly string _reposPath;

        public GitSourceControlProvider(string gitPath, string _reposPath)
        {
            _gitPath = gitPath;
            _reposPath = _reposPath;
        }

        public void CreateRepository(string name)
        {
            var cmd = new GitCommand(_gitPath, _reposPath);
            cmd.Execute("init --bare" + name);
        }
    }
}