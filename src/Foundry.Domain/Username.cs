
using System;
namespace Foundry.Domain
{
    [Serializable]
    public class Username
    {
        public string Value { get; private set; }

        public Username(string value)
        {
            Value = value;
        }
    }
}
