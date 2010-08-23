using System;

namespace Foundry.Reporting
{
    public class UserReport
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        internal PasswordFormat PasswordFormat { get; set; }

        internal string Password { get; set; }

        internal string Salt { get; set; }

        public bool IsValidPassword(string plainTextPassword)
        {
            var generated = GeneratePassword(PasswordFormat, plainTextPassword, Salt);
            return Password == generated;
        }

        public void SetPassword(string plainTextPassword)
        {
            Salt = GenerateSalt();
            PasswordFormat = Reporting.PasswordFormat.Plain;
            Password = GeneratePassword(PasswordFormat, plainTextPassword, Salt);
        }

        private static string GeneratePassword(PasswordFormat passwordFormat, string plainTextPassword, string salt)
        {
            return plainTextPassword;
        }

        private static string GenerateSalt()
        {
            return "salt";
        }
    }
}