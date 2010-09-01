using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Foundry.SourceControl.GitIntegration
{
    public abstract class GitCommand : IGitCommand
    {
        private readonly IGitSession _session;

        public StreamReader Input { get; set; }

        public StreamWriter Output { get; set; }

        public StreamWriter Error { get; set; }

        public abstract string Name { get; }

        public IGitSession Session
        {
            get { return _session; }
        }

        public GitCommand(IGitSession session)
        {
            _session = session;
        }

        public void Execute()
        {
            var processInfo = new ProcessStartInfo(_session.Path)
            {
                Arguments = BuildCommandLine(GetArguments()),
                CreateNoWindow = true,
                RedirectStandardInput = Input != null,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = _session.WorkingDirectory
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