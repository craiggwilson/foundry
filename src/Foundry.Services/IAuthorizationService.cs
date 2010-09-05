using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Services
{
    public interface IAuthorizationService
    {
        AuthorizationInformation GetAuthorizationInformation(Guid userId);
    }
}