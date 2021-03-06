﻿using System;

namespace Foundry.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }
    }
}