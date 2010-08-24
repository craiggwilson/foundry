using System;

namespace Foundry.Reporting
{
    public class UserReport
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public int PasswordFormat { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public bool IsValidPassword(string plainTextPassword)
        {
            var generated = GeneratePassword(PasswordFormat, plainTextPassword, Salt);
            return Password == generated;
        }

        public void SetPassword(string plainTextPassword)
        {
            Salt = GenerateSalt();
            PasswordFormat = 0;
            Password = GeneratePassword(PasswordFormat, plainTextPassword, Salt);
        }

        private static string GeneratePassword(int passwordFormat, string plainTextPassword, string salt)
        {
            return plainTextPassword;
        }

        private static string GenerateSalt()
        {
            return "salt";
        }
    }
}