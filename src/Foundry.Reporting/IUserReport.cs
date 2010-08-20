using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Reporting
{
    public interface IUserReport
    {
        User FindUser(string username);
    }
}