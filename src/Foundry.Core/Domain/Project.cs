using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Security;

namespace Foundry.Domain
{
    public class Project
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string AccountName { get; set; }

        public string RepositoryName { get; set; }

        public string Name
        {
            get { return GetName(AccountName, RepositoryName); }
        }

        public string SourceControlProvider { get; set; }

        public bool IsPrivate { get; set; }

        public static string GetName(string accountName, string repositoryName)
        {
            return accountName + "/" + repositoryName;
        }
    }
}