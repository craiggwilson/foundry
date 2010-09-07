using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Foundry.Domain;
using Foundry.Domain.Infrastructure;

namespace Foundry.Domain.Infrastructure
{
    public class FoundryRepository<T> : IDomainRepository<T>, IQueryable<T> where T : class
    {
        private readonly IDomainSession _session;

        public FoundryRepository(IDomainSession session)
        {
            _session = session;
        }

        public void Add(T report)
        {
            _session.Add(report);
        }

        public void Remove(T report)
        {
            _session.Remove(report);
        }

        #region IQueryable<T>
        
        Type IQueryable.ElementType
        {
            get { return ((IQueryable)_session.Query<T>()).ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get {return ((IQueryable)_session.Query<T>()).Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return ((IQueryable)_session.Query<T>()).Provider; }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IQueryable<T>)_session.Query<T>()).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IQueryable)_session.Query<T>()).GetEnumerator();
        }

        #endregion
    }
}
