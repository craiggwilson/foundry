using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Domain
{
    [Serializable]
    public class Password
    {
        public string Value { get; private set; }

        public PasswordFormat Format { get; private set; }

        public string Salt { get; private set; }

        public Password(string value)
        {
            Format = PasswordFormat.Plain;
            Salt = GenerateSalt();
            Value = GeneratePassword(Format, value, Salt);
        }

        public Password(string value, PasswordFormat format, string salt)
        {
            Format = format;
            Salt = salt;
            Value = value;
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