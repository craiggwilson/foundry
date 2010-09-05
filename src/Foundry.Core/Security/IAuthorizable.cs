using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Security
{
    public interface IAuthorizable<T>
    {
        Guid Id { get; }
    }
}