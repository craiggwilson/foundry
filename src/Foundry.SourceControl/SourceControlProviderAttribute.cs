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

        public SourceControlProviderAttribute(string name)
        {
            Name = name;
        }
    }
}