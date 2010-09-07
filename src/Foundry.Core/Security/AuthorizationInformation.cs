using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Foundry.Security
{
    public class AuthorizationInformation
    {
        private readonly Guid _userId;
        private readonly IEnumerable<UserAuthorization> _authorizations;

        public Guid UserId
        {
            get { return _userId; }
        }

        public AuthorizationInformation(Guid userId, IEnumerable<UserAuthorization> authorizations)
        {
            _userId = userId;
            _authorizations = authorizations;
        }

        public bool IsAllowed(string subjectType, Guid subjectId, string operation)
        {
            //TODO: come back and do this...
            return true;
        }

        public IQueryable<T> Filter<T>(IQueryable<T> subjects, Expression<Func<T, Guid>> idSelector, string subjectType, string operation)
        {
            var auths = GetDistinct(subjectType, operation).OrderByDescending(x => x.Level);

            if (!auths.Any())
                return Enumerable.Empty<T>().AsQueryable();

            var allowedList = new HashSet<Guid>();
            var denyList = new HashSet<Guid>();

            bool allowDefault = false;
            foreach (var perm in auths)
            {
                if (perm.Allow)
                {
                    if (perm.SubjectId == Guid.Empty)
                    {
                        allowDefault = true;
                        break;
                    }

                    if(!allowedList.Contains(perm.SubjectId) && !denyList.Contains(perm.SubjectId))
                        allowedList.Add(perm.SubjectId);
                }
                else
                {
                    if (perm.SubjectId == Guid.Empty)
                    {
                        allowDefault = false;
                        break;
                    }

                    if(!allowedList.Contains(perm.SubjectId) && !denyList.Contains(perm.SubjectId))
                        denyList.Add(perm.SubjectId);
                }
            }

            if (allowDefault)
            {
                if (denyList.Any())
                    subjects = subjects.Where(BuildInClause(denyList.AsQueryable(), false, idSelector));
            }
            else
            {
                if (allowedList.Any())
                    subjects = subjects.Where(BuildInClause(allowedList.AsQueryable(), true, idSelector));
                else
                    return Enumerable.Empty<T>().AsQueryable();
            }

            return subjects;
        }

        private IEnumerable<UserAuthorization> GetDistinct(string subjectType, string operation)
        {
            return from p in _authorizations
                   where p.SubjectType == subjectType && p.Operation == operation
                   select p;
        }

        private static Expression<Func<T, bool>> BuildInClause<T>(IQueryable<Guid> guids, bool allow, Expression<Func<T, Guid>> idSelector)
        {
            var itemParameter = idSelector.Parameters.Single();

            var lambda = idSelector.Body;

            Expression expression = Expression.Call(
                typeof(Queryable),
                "Contains",
                new[] { typeof(Guid) },
                Expression.Constant(guids, typeof(IQueryable<Guid>)),
                idSelector.Body);

            if (!allow)
                expression = Expression.Not(expression);

            return Expression.Lambda<Func<T, bool>>(expression, itemParameter);
        }
    }
}