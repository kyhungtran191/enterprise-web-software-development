using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.Identity.Users.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<ErrorOr<IResponseWrapper<string>>>
    {
        public string Email { get; set; }
    }
}
