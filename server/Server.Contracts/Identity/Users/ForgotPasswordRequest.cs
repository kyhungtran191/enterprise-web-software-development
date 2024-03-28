using System.ComponentModel.DataAnnotations;

namespace Server.Contracts.Identity.Users
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
