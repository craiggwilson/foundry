using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitSourceFile : GitSourceObject, ISourceFile
    {
        public string Extension
        {
            get
            {
                var ext = System.IO.Path.GetExtension(Name);
                if (!string.IsNullOrWhiteSpace(ext) && ext.StartsWith("."))
                    ext = ext.Substring(1);
                return ext;
            }
        }

        public byte[] Content { get; set; }
    }
}