using System.Data.Entity;
using Foundry.Reporting;

namespace Foundry.Reporting.Infrastructure
{
    public class ReportingDbContext : DbContext
    {
        public ReportingDbContext()
            : base("Reporting")
        {
            Database.CreateIfNotExists();
        }

        protected override void OnModelCreating(System.Data.Entity.ModelConfiguration.ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.RegisterSet<UserReport>();
        }
    }
}
