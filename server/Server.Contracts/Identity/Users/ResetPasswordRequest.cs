using System.ComponentModel.DataAnnotations;

namespace Server.Contracts.Identity.Users
{
    public class ResetPasswordRequest
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Token { get; set; }
        
    }
}
