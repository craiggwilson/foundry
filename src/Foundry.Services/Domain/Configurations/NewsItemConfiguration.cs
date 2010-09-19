using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using Foundry.Domain;

namespace Foundry.Services.Domain.Configurations
{
    public class NewsItemConfiguration : EntityConfiguration<NewsItem>
    {
        public NewsItemConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id).IsIdentity();
            Property(x => x.Message).IsRequired().HasMaxLength(255);
            Property(x => x.Event).IsRequired().HasMaxLength(255);
            Property(x => x.DateTime);
            Property(x => x.UserId);
            Property(x => x.Username).HasMaxLength(255);
            Property(x => x.UserDisplayName).HasMaxLength(255);
            Property(x => x.ProjectId);
            Property(x => x.ProjectFullName).HasMaxLength(255);
        }
    }
}