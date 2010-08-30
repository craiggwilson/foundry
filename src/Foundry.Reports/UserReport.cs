using System;

namespace Foundry.Reports
{
    public class UserReport
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? LastLoginDateTime { get; set; }
    }
}