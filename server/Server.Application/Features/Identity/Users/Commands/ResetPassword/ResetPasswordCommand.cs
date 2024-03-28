using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.Identity.Users.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public string Token { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
