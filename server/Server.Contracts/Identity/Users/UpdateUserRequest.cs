namespace Server.Contracts.Identity.Users;

public class UpdateUserRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? Dob { get; set; }
    public string? Avatar { get; set; }
    public bool IsActive { get; set; }

    public class MappingProfiles
    {

    }
}