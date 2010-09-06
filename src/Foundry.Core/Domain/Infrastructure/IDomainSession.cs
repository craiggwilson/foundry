using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Domain.Infrastructure
{
    public interface IDomainSession
    {
        void Add<T>(T entity) where T : class;

        void Remove<T>(T entity) where T : class;

        IQueryable<T> Query<T>() where T : class;

        void Commit();
    }
}
