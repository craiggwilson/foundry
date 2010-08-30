using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Reports;

namespace Foundry.Services
{
    public interface IMembershipService
    {
        bool CreateUser(string username, string password, string displayName, string email);

        Tuple<bool, UserReport> TryLogin(string username, string password);
    }
}