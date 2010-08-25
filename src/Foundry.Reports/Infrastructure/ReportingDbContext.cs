using System.Data.Entity;
using Foundry.Reporting;

namespace Foundry.Reporting.Infrastructure
{
    public class ReportingDbContext : DbContext, IReportingUnitOfWork
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
            modelBuilder.Entity<UserReport>()
                .HasKey(x => x.Id);
        }

        public void Commit()
        {
            this.SaveChanges();
        }
    }
}
