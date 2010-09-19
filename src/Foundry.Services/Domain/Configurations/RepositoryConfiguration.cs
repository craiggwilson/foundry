using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using Foundry.Domain;

namespace Foundry.Services.Domain.Configurations
{
    public class RepositoryConfiguration : EntityConfiguration<Project>
    {
        public RepositoryConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id);
            Property(x => x.AccountName).IsRequired().HasMaxLength(50);
            Property(x => x.RepositoryName).IsRequired().HasMaxLength(50);
            Property(x => x.SourceControlProvider).IsRequired().HasMaxLength(32);
            Property(x => x.AccountId);
            Property(x => x.IsPrivate);
        }
    }
}
