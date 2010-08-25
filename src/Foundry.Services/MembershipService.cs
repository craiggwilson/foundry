using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Foundry.Reporting;
using Foundry.Messaging;

namespace Foundry.Services
{
    public class MembershipService : ServiceBase, IMembershipService
    {
        private IBus _bus;
        private IReportingRepository<UserReport> _userRepository;

        public MembershipService(IBus bus, IReportingRepository<UserReport> userRepository)
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

            WithTransaction(() => _bus.Send(new CreateUserMessage { DisplayName = displayName, Email = email, Password = password, PasswordSalt = salt, Username = username }));

            return true;
        }

        public Tuple<bool, UserReport> TryLogin(string username, string plainTextPassword)
        {
            var user = _userRepository.SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                WithTransaction(() => _bus.Send(new UserAuthenticationFailedMessage { Username = username }));
                return Tuple.Create<bool, UserReport>(false, null);
            }

            var password = GeneratePassword(plainTextPassword, user.Salt);
            if (user.Password != password)
            {
                WithTransaction(() => _bus.Send(new UserAuthenticationFailedMessage { Username = username}));
                return Tuple.Create<bool, UserReport>(false, null);
            }

            WithTransaction(() => _bus.Send(new UserLoggedInMessage { UserId = user.Id }));

            return Tuple.Create(true, user);
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
