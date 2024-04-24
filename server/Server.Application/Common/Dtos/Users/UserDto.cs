using AutoMapper;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Server.Domain.Entity.Identity;

namespace Server.Application.Common.Dtos.Users;
public class UserDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Faculty { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateCreated { get; set; }
    public bool IsActive { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
    public DateTime? Dob { get; set; }
    public string? Avatar { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserDto>();
        }
    }
}