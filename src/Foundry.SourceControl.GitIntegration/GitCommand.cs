using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Foundry.SourceControl.GitIntegration
{
    internal class GitCommand
    {
        private readonly string _gitPath;
        private readonly string _workingDirectory;

        public GitCommand(string gitPath, string workingDirectory)
        {
            _gitPath = gitPath;
            _workingDirectory = workingDirectory;
        }

        public string Execute(string args)
        {
            var processInfo = new ProcessStartInfo(_gitPath)
            {
                Arguments = args,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = _workingDirectory
            };

            using (var process = Process.Start(processInfo))
            {
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                return output;
            };
        }
    }
}