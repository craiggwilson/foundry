
using System.ComponentModel.DataAnnotations;
namespace Foundry.Website.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is required to login.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required to login.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required to register.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is required to register.")]
        public string PasswordAgain { get; set; }

        [Required(ErrorMessage = "Display Name is required to login.")]
        public string DisplayName { get; set; }
    }
}