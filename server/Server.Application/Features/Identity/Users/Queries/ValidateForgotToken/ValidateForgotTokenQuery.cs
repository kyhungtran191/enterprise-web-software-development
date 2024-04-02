using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.Identity.Users.Queries.ValidateForgotToken
{
    public class ValidateForgotTokenQuery : IRequest<ErrorOr<IResponseWrapper<bool>>>
    {
        public string Token { get; set; }
    }
}
