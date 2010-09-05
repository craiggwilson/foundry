using System.Data.Entity;

namespace Foundry.Reports.Infrastructure
{
    public class ReportingDbContext : DbContext, IReportingSession
    {
        public ReportingDbContext()
            : base("Reporting")
        {
            Database.CreateIfNotExists();
        }

        public void Add<T>(T report) where T : class
        {
            this.Set<T>().Add(report);
        }

        public void Remove<T>(T report) where T : class
        {
            this.Set<T>().Remove(report);
        }

        public System.Linq.IQueryable<T> Query<T>()  where T : class
        {
            return this.Set<T>();
        }

        public void Commit()
        {
            this.SaveChanges();
        }

        protected override void OnModelCreating(System.Data.Entity.ModelConfiguration.ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.RegisterSet<UserReport>();
            modelBuilder.Entity<UserReport>()
                .HasKey(x => x.UserId);

            modelBuilder.RegisterSet<RepositoryReport>();
            modelBuilder.Entity<RepositoryReport>()
                .HasKey(x => x.RepositoryId);

            modelBuilder.RegisterSet<NewsFeedReport>();
            modelBuilder.Entity<NewsFeedReport>()
                .HasKey(x => x.SubjectId);

            modelBuilder.RegisterSet<UserPermissionsReport>();
            modelBuilder.Entity<UserPermissionsReport>()
                .HasKey(x => new { x.UserId, x.SubjectId, x.Operation });
        }
    }
}