
using System;
namespace Foundry.Domain
{
    [Serializable]
    public class Email
    {
        public string Address { get; private set; }

        public Email(string address)
        {
            Address = address;
        }
    }
}
