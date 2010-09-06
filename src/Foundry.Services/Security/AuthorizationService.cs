using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Reports;
using Foundry.Security;

namespace Foundry.Services.Security
{
    public class AuthorizationService : IAuthorizationService
    {
        private static readonly List<OperationImplication> _implications = new List<OperationImplication>
        {
            new OperationImplication("Create", "Create"),
            new OperationImplication("Create", "Read"),
            new OperationImplication("Delete", "Delete"),
            new OperationImplication("Delete", "Read"),
            new OperationImplication("Write", "Write"),
            new OperationImplication("Write", "Read"),
            new OperationImplication("*", "Create"),
            new OperationImplication("*", "Read"),
            new OperationImplication("*", "Write"),
            new OperationImplication("*", "Delete"),
        };

        private readonly IReportingRepository<UserPermissionsReport> _permissionsRepository;

        public AuthorizationService(IReportingRepository<UserPermissionsReport> permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public AuthorizationInformation GetAuthorizationInformation(Guid userId)
        {
            var userPermissions = _permissionsRepository
                .Where(x => x.UserId == userId || x.UserId == Guid.Empty)
                .ToList();

            return new AuthorizationInformation(
                userId,
                from p in userPermissions
                join oi in _implications on p.Operation equals oi.Operation into g
                from perm in g.DefaultIfEmpty(new OperationImplication(p.Operation, p.Operation))
                select new UserPermission
                {
                    SubjectType = p.SubjectType,
                    SubjectId = p.SubjectId,
                    Operation = perm.ImpliedOperation,
                    Level = p.Level,
                    Allow = p.Allow
                });
        }

        private class OperationImplication
        {
            public string Operation { get; private set; }

            public string ImpliedOperation { get; private set; }

            public OperationImplication (string operation, string impliedOperation)
	        {
                Operation = operation;
                ImpliedOperation = impliedOperation;
	        }
        }
    }
}