
namespace Foundry.Messaging.Messages
{
    public class CreateUserMessage
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public PasswordFormat PasswordFormat { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

    }
}