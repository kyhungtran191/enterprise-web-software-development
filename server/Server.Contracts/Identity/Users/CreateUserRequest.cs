using Microsoft.AspNetCore.Http;

namespace Server.Contracts.Identity.Users;

public class CreateUserRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    // public string Password { get; set; } = default!;
    public Guid FacultyId { get; set; }
    public Guid RoleId { get; set; } = default!;
    public DateTime? Dob { get; set; }
    public IFormFile? Avatar { get; set; }
    public bool IsActive { get; set; }
}