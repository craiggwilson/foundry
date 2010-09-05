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