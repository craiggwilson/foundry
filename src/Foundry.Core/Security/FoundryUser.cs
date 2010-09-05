using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace Foundry.Security
{
    public class FoundryUser : IPrincipal, IIdentity
    {
        public static readonly FoundryUser Anonymous = new FoundryUser
        {
            IsAuthenticated = false
        };

        public Guid Id { get; set; }

        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public FoundryUser()
        { }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}", Id, AuthenticationType, IsAuthenticated, Name, DisplayName);
        }

        public static FoundryUser FromString(string data)
        {
            var parts = data.Split('|');
            return new FoundryUser
            {
                Id = Guid.Parse(parts[0]),
                AuthenticationType = parts[1],
                IsAuthenticated = bool.Parse(parts[2]),
                Name = parts[3],
                DisplayName = parts[4]
            };
        }

        public IIdentity Identity
        {
            get { return this; }
        }

        public bool IsInRole(string role)
        {
            return false;
        }
    }
}