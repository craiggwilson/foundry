using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Reports;

namespace Foundry.Services
{
    public class AuthorizationInformation
    {
        private readonly Guid _userId;
        private readonly IEnumerable<UserPermissionsReport> _permissions;

        public Guid UserId
        {
            get { return _userId; }
        }

        public AuthorizationInformation(Guid userId, IEnumerable<UserPermissionsReport> permissions)
        {
            _userId = userId;
            _permissions = permissions;
        }

        public bool IsAllowed(SubjectType subjectType, Guid subjectId, string operation)
        {
            //TODO: come back and do this...
            return true;
        }

        public IEnumerable<UserPermissionsReport> GetAllAuthorizations(SubjectType subjectType, string operation)
        {
            var entries = _permissions.Where(x => x.SubjectType == subjectType);
            //TODO: come back and do this...
            return entries;
        }
    }
}