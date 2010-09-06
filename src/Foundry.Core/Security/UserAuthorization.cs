using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Security
{
    public class UserAuthorization
    {
        public string SubjectType { get; set; }

        public Guid SubjectId { get; set; }

        public string Operation { get; set; }

        public bool Allow { get; set; }

        public int Level { get; set; }
    }
}
