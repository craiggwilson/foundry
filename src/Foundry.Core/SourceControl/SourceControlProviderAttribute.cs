using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Foundry.SourceControl
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SourceControlProviderAttribute : ExportAttribute
    {
        public string Name { get; private set; }

        public bool CommitsHaveParents { get; set; }

        public SourceControlProviderAttribute(string name)
            : base(typeof(ISourceControlProvider))
        {
            Name = name;
        }
    }
}