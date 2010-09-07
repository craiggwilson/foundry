using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Foundry.Security
{
    public interface IAuthorizationService
    {
        AuthorizationInformation GetAuthorizationInformation(Guid userId);
    }
}