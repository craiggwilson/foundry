using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration.Commands
{
    public class GitUploadPackCommand : GitCommand
    {
        public string Directory { get; private set; }

        public override string Name
        {
            get { return "upload-pack"; }
        }

        public bool? Strict { get; set; }

        public TimeSpan? Timeout { get; set; }

        public GitUploadPackCommand(IGitSession session, string directory)
            : base(session)
        {
            Directory = directory;
        }

        protected override IEnumerable<string> GetArguments()
        {
            if (Strict.GetValueOrDefault(false))
                yield return "--strict";

            if (Timeout.HasValue)
                yield return "--timeout=" + Timeout.Value.Seconds.ToString();

            yield return Directory;
        }
    }
}