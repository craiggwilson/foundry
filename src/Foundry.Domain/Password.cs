using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Domain
{
    public class Password
    {
        public string Value { get; private set; }

        public PasswordFormat Format { get; private set; }

        public string Salt { get; private set; }

        public Password(string value, PasswordFormat format, string salt)
        {
            Value = value;
            Format = format;
            Salt = salt;
        }
    }
}