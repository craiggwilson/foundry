using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ISourceControlManager
    {
        IEnumerable<string> ProviderNames { get; }

        void CreateUserRepository(Guid userId, string providerName, string accountName, string projectName);
    }
}