
using System.ComponentModel.DataAnnotations;

namespace CarMechanic.Shared.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "The email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is requierd.")]
        public string Password { get; set; } = string.Empty;
    }
}
