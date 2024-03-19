using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.Identity.Users.Commands;

public class UserCommandBase : IRequest<ErrorOr<IResponseWrapper>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? Dob { get; set; }
    public string? Avatar { get; set; }
    public bool IsActive { get; set; }
}