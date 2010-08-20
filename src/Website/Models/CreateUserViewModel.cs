using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundry.Website.Models
{
    public class CreateUserViewModel
    {
        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }
    }
}