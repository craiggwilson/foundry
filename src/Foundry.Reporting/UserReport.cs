using System;

namespace Foundry.Reporting
{
    public class UserReport
    {
        private string _hashedPassword;
        private string _salt;
        private PasswordFormat _passwordFormat;

        public Guid Id { get; set; }

        public string Username { get; set; }

        public bool IsValidPassword(string plainTextPassword)
        {
            return true;
        }

        public static string HashPassword(PasswordFormat passwordFormat, string plainTextPassword, string salt)
        {
            return plainTextPassword;
        }

        public static string CreateSalt()
        {
            return "salt";
        }
    }
}