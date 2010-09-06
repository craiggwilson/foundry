using System.Data.Entity;
using Foundry.Domain;

namespace Foundry.Domain.Infrastructure
{
    public class FoundryDbContext : DbContext, IDomainSession
    {
        public FoundryDbContext()
            : base("Foundry")
        { }

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

            modelBuilder.RegisterSet<User>();
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.RegisterSet<Repository>();
            modelBuilder.Entity<Repository>()
                .HasKey(x => x.Id);

            modelBuilder.RegisterSet<NewsItem>();
            modelBuilder.Entity<NewsItem>()
                .HasKey(x => x.SubjectId);

            modelBuilder.RegisterSet<UserPermission>();
            modelBuilder.Entity<UserPermission>()
                .HasKey(x => new { x.UserId, x.SubjectId, x.Operation });
        }
    }
}