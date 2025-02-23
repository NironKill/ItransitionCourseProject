using System.ComponentModel.DataAnnotations;

namespace CustomForms.Models.Account
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
