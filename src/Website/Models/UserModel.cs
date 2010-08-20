using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundry.Website.Models
{
    public class UserModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}