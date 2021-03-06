﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using Foundry.Domain;

namespace Foundry.Services.Domain.Configurations
{
    public class UserConfiguration : EntityConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id);
            Property(x => x.Username).IsRequired().HasMaxLength(255);
            Property(x => x.Password).IsRequired().HasMaxLength(32);
            Property(x => x.Salt).IsRequired().HasMaxLength(32);
            Property(x => x.Email).IsRequired().HasMaxLength(255);
            Property(x => x.DisplayName).IsRequired().HasMaxLength(255);
        }
    }
}
