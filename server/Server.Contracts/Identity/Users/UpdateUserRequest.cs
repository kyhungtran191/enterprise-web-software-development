using Microsoft.AspNetCore.Http;

namespace Server.Contracts.Identity.Users;

public class UpdateUserRequest
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid FacultyId { get; set; }
    public Guid RoleId { get; set; }    
    public DateTime? Dob { get; set; }
    public IFormFile? Avatar { get; set; }
    public bool IsActive { get; set; }
}