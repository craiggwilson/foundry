using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Foundry.Reporting;

namespace Foundry.Infrastructure
{
    public class EfReportingRepository<T> : IReportingRepository<T>, IQueryable<T> where T : class
    {
        private readonly IDbSet<T> _set;

        public EfReportingRepository(DbContext context)
        {
            _set = context.Set<T>();
        }

        public void Add(T report)
        {
            _set.Add(report);
        }

        public void Remove(T report)
        {
            _set.Remove(report);
        }

        #region IQueryable<T>
        
        Type IQueryable.ElementType
        {
            get { return ((IQueryable)_set).ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get {return ((IQueryable)_set).Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return ((IQueryable)_set).Provider; }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IQueryable<T>)_set).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IQueryable)_set).GetEnumerator();
        }

        #endregion
    }
}
