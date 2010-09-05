using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Security
{
    public class AuthorizationInformation
    {
        private readonly Guid _userId;
        private readonly IEnumerable<UserPermission> _permissions;

        public Guid UserId
        {
            get { return _userId; }
        }

        public AuthorizationInformation(Guid userId, IEnumerable<UserPermission> permissions)
        {
            _userId = userId;
            _permissions = permissions;
        }

        public bool IsAllowed(string subjectType, Guid subjectId, string operation)
        {
            //TODO: come back and do this...
            return true;
        }

        public IEnumerable<T> Filter<T>(IEnumerable<T> subjects, string subjectType, string operation) where T : IAuthorizable<T>
        {
            var perms = GetDistinctPermissions(subjectType, operation).OrderByDescending(x => x.Level);
            
            if (!perms.Any())
                return Enumerable.Empty<T>();

            var allowedList = new List<Guid>();
            var denyList = new List<Guid>();

            bool allowDefault = false;
            foreach (var perm in perms)
            {
                if (perm.Allow)
                {
                    if (perm.SubjectId == Guid.Empty)
                    {
                        allowDefault = true;
                        break;
                    }

                    allowedList.Add(perm.SubjectId);
                }
                else
                {
                    if (perm.SubjectId == Guid.Empty)
                    {
                        allowDefault = false;
                        break;
                    }

                    denyList.Add(perm.SubjectId);
                }
            }

            if (allowDefault)
            {
                if (denyList.Any())
                    subjects = subjects.Where(s => !denyList.Contains(s.Id));
            }
            else
            {
                if (allowedList.Any())
                    subjects = subjects.Where(s => allowedList.Contains(s.Id));
                else
                    return Enumerable.Empty<T>();
            }

            return subjects;
        }

        private IEnumerable<UserPermission> GetDistinctPermissions(string subjectType, string operation)
        {
            return from p in _permissions
                   where p.SubjectType == subjectType && p.Operation == operation
                   group p by p.SubjectId into g
                   select g.OrderByDescending(x => x.Level).First();
        }
    }
}