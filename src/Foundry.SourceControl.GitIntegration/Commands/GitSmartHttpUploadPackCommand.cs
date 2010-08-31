using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration.Commands
{
    public class GitSmartHttpUploadPackCommand : GitUploadPackCommand
    {
        public bool? AdvertiseRefs { get; set; }

        public GitSmartHttpUploadPackCommand(IGitSession session, string directory)
            : base(session, directory)
        { }

        protected override IEnumerable<string> GetArguments()
        {
            yield return "--stateless-rpc";
            if(AdvertiseRefs.GetValueOrDefault())
                yield return "--advertise-refs";
            foreach (var arg in base.GetArguments())
                yield return arg;
        }

    }
}