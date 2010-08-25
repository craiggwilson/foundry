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

        public string Salt { get; private set; }

        public Password(string value, string salt)
        {
            Salt = salt;
            Value = value;
        }
    }
}