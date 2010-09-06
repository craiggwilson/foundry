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
        }
    }

    public class RepositoryNewsItemConfiguration : EntityConfiguration<RepositoryNewsItem>
    {
        public RepositoryNewsItemConfiguration()
        {
            Property(x => x.RepositoryId);
            Property(x => x.RepositoryName).HasMaxLength(255);
        }
    }

    public class UserNewsItemConfiguration : EntityConfiguration<UserNewsItem>
    {
        public UserNewsItemConfiguration()
        {
            Property(x => x.UserId);
            Property(x => x.Username).HasMaxLength(255);
            Property(x => x.UserDisplayName).HasMaxLength(255);
        }
    }
}