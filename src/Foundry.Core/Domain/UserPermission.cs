using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Domain
{
    public class UserPermission
    {
        public Guid UserId { get; set; }

        public string SubjectType { get; set; }

        public Guid SubjectId { get; set; }

        public string Operation { get; set; }

        public int Level { get; set; }

        public bool Allow { get; set; }
    }
}