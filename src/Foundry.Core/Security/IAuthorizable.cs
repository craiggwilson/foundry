using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Security
{
    public interface IAuthorizable
    {
        Guid Id { get; }
    }
}