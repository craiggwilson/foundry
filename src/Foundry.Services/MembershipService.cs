using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Foundry.Reports;
using Foundry.Messaging;
using Foundry.Reports.Infrastructure;

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

            _bus.Send(new CreateUserMessage { DisplayName = displayName, Email = email, Password = password, PasswordSalt = salt, Username = username });

            return true;
        }

        public Tuple<bool, UserReport> TryLogin(string username, string plainTextPassword)
        {
            var user = _userRepository.SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                _bus.Send(new UserAuthenticationFailedMessage { Username = username });
                return Tuple.Create<bool, UserReport>(false, null);
            }

            var password = GeneratePassword(plainTextPassword, user.Salt);
            if (user.Password != password)
            {
                _bus.Send(new UserAuthenticationFailedMessage { Username = username });
                return Tuple.Create<bool, UserReport>(false, null);
            }

            _bus.Send(new UserLoggedInMessage { UserId = user.UserId });

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
