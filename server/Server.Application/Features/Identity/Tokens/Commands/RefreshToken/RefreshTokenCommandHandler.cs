using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Authentication;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Tokens.Commands.RefreshToken;

public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, ErrorOr<IResponseWrapper<RefreshTokenResult>>>
{
    private readonly ITokenService _tokenService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserManager<AppUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RefreshTokenCommandHandler(UserManager<AppUser> userManager,
                                      ITokenService tokenService,
                                      IDateTimeProvider dateTimeProvider,
                                      IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _dateTimeProvider = dateTimeProvider;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<IResponseWrapper<RefreshTokenResult>>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);

        if (string.IsNullOrWhiteSpace(principal?.Identity?.Name))
        {
            return Errors.Token.Invalid;
        }

        var name = principal.Identity.Name;

        var userOfToken = await _userManager.FindByNameAsync(name);

        if (userOfToken is null)
        {
            return Errors.User.CannotFound;
        }

        if (request.RefreshToken != userOfToken.RefreshToken)
        {
            return Errors.RefreshToken.NotMatch;
        }

        if (userOfToken.RefreshTokenExpiryTime <= _dateTimeProvider.UtcNow)
        {
            return Errors.Token.Expired;
        }

        var newAccessToken = await _jwtTokenGenerator.GenerateToken(userOfToken);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        userOfToken.RefreshToken = newRefreshToken;

        var result = await _userManager.UpdateAsync(userOfToken);

        if (!result.Succeeded)
        {
            return result.GetIdentityResultErrorDescriptions();
        }

        return new ResponseWrapper<RefreshTokenResult>
        {
            IsSuccessfull = true,
            Messages = new List<string> { "Refresh token successfully!" },
            ResponseData = new RefreshTokenResult
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            }
        };
    }
}