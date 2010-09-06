using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Domain
{
    public interface IDomainRepository<T> : IQueryable<T> where T : class
    {
        void Add(T entity);

        void Remove(T entity);
    }
}
