using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using Foundry.Domain;

namespace Foundry.Services.Domain.Configurations
{
    public class UserPermissionConfiguration : EntityConfiguration<UserPermission>
    {
        public UserPermissionConfiguration()
        {
            HasKey(x => new { x.UserId, x.SubjectId, x.SubjectType, x.Operation, x.Level });

            Property(x => x.UserId);
            Property(x => x.SubjectId);
            Property(x => x.SubjectType).HasMaxLength(255);
            Property(x => x.Operation).IsRequired().HasMaxLength(255);
            Property(x => x.Level);
            Property(x => x.Allow);
        }
    }
}
