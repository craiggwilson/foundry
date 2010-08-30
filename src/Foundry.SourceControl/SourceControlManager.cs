using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Foundry.SourceControl
{
    [Export]
    public class SourceControlManager : ISourceControlManager
    {
        private IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>> _providers;

        public IEnumerable<string> ProviderNames
        {
            get { return _providers.Select(x => x.Metadata.Name); }
        }

        public SourceControlManager(IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>> providers)
        {
            _providers = providers;
        }

        public ISourceControlProvider GetByName(string name)
        {
            return _providers.Single(x => x.Metadata.Name == name).Value;
        }
    }
}