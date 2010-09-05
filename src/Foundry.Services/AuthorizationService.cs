using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Reports;

namespace Foundry.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IReportingRepository<UserPermissionsReport> _permissionsRepository;

        public AuthorizationService(IReportingRepository<UserPermissionsReport> permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public AuthorizationInformation GetAuthorizationInformation(Guid userId)
        {
            return new AuthorizationInformation(
                userId,
                _permissionsRepository
                    .Where(x => x.UserId == Guid.Empty || x.UserId == userId)
                    .OrderByDescending(x => x.Level));
        }
    }
}