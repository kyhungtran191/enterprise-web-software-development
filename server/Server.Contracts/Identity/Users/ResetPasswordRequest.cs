using System.ComponentModel.DataAnnotations;

namespace Server.Contracts.Identity.Users
{
    public class ResetPasswordRequest
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
