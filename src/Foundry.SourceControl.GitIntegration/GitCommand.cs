using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Foundry.SourceControl.GitIntegration
{
    public abstract class GitCommand : IGitCommand
    {
        private readonly IGitSession _session;

        public string Infile { get; set; }

        public string Outfile { get; set; }

        public abstract string Name { get; }

        public IGitSession Session
        {
            get { return _session; }
        }

        public GitCommand(IGitSession session)
        {
            _session = session;
        }

        public GitCommandResult Execute()
        {
            var processInfo = new ProcessStartInfo(_session.Path)
            {
                Arguments = BuildCommandLine(GetArguments()),
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = _session.WorkingDirectory
            };

            using (var process = Process.Start(processInfo))
            {
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                return new GitCommandResult(output, error);
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

            if (!string.IsNullOrWhiteSpace(Infile))
                sb.AppendFormat(" < {0}", Quote(Infile));

            if(!string.IsNullOrWhiteSpace(Outfile))
                sb.AppendFormat(" > {0}", Quote(Outfile));

            return sb.ToString();
        }
    }
}