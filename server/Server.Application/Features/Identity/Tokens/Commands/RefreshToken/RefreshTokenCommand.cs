using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.Identity.Tokens.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<ErrorOr<IResponseWrapper<RefreshTokenResult>>>
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}