using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Foundry.Messaging;
using Foundry.Security;
using Foundry.Domain;

namespace Foundry.Services.Security
{
    public class MembershipService : ServiceBase, IMembershipService
    {
        private IBus _bus;
        private IDomainRepository<User> _userRepository;

        public MembershipService(IBus bus, IDomainRepository<User> userRepository)
        {
            _bus = bus;
            _userRepository = userRepository;
        }

        public bool CreateUser(string username, string plainTextPassword, string displayName, string email)
        {
            var user = _userRepository.SingleOrDefault(u => u.Username == username);
            if (user != null)
                return false;

            var salt = GenerateSalt();
            var password = GeneratePassword(plainTextPassword, salt);

            _bus.Send(new CreateUserMessage { DisplayName = displayName, Email = email, Password = password, PasswordSalt = salt, Username = username });

            return true;
        }

        public FoundryUser TryLogin(string username, string plainTextPassword)
        {
            var user = _userRepository.SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                _bus.Send(new UserAuthenticationFailedMessage { Username = username });
                return FoundryUser.Anonymous;
            }

            var password = GeneratePassword(plainTextPassword, user.Salt);
            if (user.Password != password)
            {
                _bus.Send(new UserAuthenticationFailedMessage { Username = username });
                return FoundryUser.Anonymous;
            }

            _bus.Send(new UserLoggedInMessage { UserId = user.Id, DateTime = DateTime.UtcNow });

            return new FoundryUser { Id = user.Id, AuthenticationType = "Forms", DisplayName = user.DisplayName, IsAuthenticated = true, Name = username };
        }

        private static string GeneratePassword(string plainTextPassword, string salt)
        {
            return plainTextPassword;
        }

        private static string GenerateSalt()
        {
            return "salt";
        }
    }
}
