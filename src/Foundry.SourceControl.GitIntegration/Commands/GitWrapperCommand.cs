using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Foundry.SourceControl.GitIntegration.Commands
{
    public abstract class GitWrapperCommand : IGitWrapperCommand
    {
        public string WorkingDirectory { get; set; }

        public string GitExePath { get; set; }

        public StreamReader Input { get; set; }

        public StreamWriter Output { get; set; }

        public StreamWriter Error { get; set; }

        public abstract string Name { get; }

        protected GitWrapperCommand()
        {
            GitExePath = GitSettings.ExePath;
            WorkingDirectory = GitSettings.RepositoriesPath;
        }

        public void Execute()
        {
            var processInfo = new ProcessStartInfo(GitExePath)
            {
                Arguments = BuildCommandLine(GetArguments()),
                CreateNoWindow = true,
                RedirectStandardInput = Input != null,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = WorkingDirectory
            };

            using (var process = Process.Start(processInfo))
            {
                if (Input != null)
                    process.StandardInput.Write(Input.ReadToEnd());

                var output = process.StandardOutput;
                var error = process.StandardError;
                process.WaitForExit();

                if (Output != null)
                    Output.Write(output.ReadToEnd());
                if (Error != null)
                    Error.Write(error.ReadToEnd());
                else
                {
                    string errorString = error.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(errorString))
                        throw new Exception(errorString);
                }
            };
        }

        protected abstract IEnumerable<string> GetArguments();

        protected string Quote(string s)
        {
            return string.Format("\"{0}\"", s);
        }

        private string BuildCommandLine(IEnumerable<string> args)
        {
            var sb = new StringBuilder();
            sb.Append(Name);

            foreach (var arg in args)
                sb.Append(" " + arg);

            return sb.ToString();
        }
    }
}