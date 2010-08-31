using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitSession : IGitSession
    {
        private readonly string _path;
        private readonly string _workingDirectory;

        public string Path
        {
            get { return _path; }
        }

        public string WorkingDirectory
        {
            get { return _workingDirectory; }
        }

        public GitSession(string path, string workingDirectory)
        {
            _path = path;
            _workingDirectory = workingDirectory;
        }

        public void Dispose()
        {
            //nothing to dispose
        }
    }
}