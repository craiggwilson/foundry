﻿
namespace Foundry.Messaging
{
    public class CreateUserMessage
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }
    }
}