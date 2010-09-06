using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using Foundry.Domain;

namespace Foundry.Services.Domain.Configurations
{
    public class RepositoryConfiguration : EntityConfiguration<Repository>
    {
        public RepositoryConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id);
            Property(x => x.Name).IsRequired().HasMaxLength(255);
            Property(x => x.SourceControlProvider).IsRequired().HasMaxLength(32);
            Property(x => x.OwnerId);
            Property(x => x.IsPrivate);
        }
    }
}
