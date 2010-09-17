using System.Data.Entity;
using Foundry.Domain;
using System.Data.Entity.ModelConfiguration;
using Foundry.Services.Domain.Configurations;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserPermissionConfiguration());
            modelBuilder.Configurations.Add(new RepositoryConfiguration());
            modelBuilder.Configurations.Add(new NewsItemConfiguration());
        }
    }
}