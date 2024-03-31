using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Server.Application.Wrappers;

namespace Server.Application.Features.Identity.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? FacultyName { get; set; }
        public DateTime? Dob { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}
