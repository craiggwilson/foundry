using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Security
{
    public interface IMembershipService
    {
        bool CreateUser(string username, string password, string displayName, string email);

        FoundryUser TryLogin(string username, string password);
    }
}