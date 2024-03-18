using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Interfaces.Authentication;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Authentication;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<LoginResult>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ITokenService _tokenService;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator,
                             UserManager<AppUser> userManager,
                             ITokenService tokenService)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<ErrorOr<LoginResult>> Handle(LoginQuery request,
                                        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null) 
        {
            return Errors.User.CannotFound;
        }

        if (user.IsActive == false || user.LockoutEnabled)
        {
            return Errors.User.InactiveOrEnableLocked;
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var accessToken = await _jwtTokenGenerator.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.AccessToken = accessToken;
        user.RefreshToken = refreshToken;

        await _userManager.UpdateAsync(user);

        return new LoginResult(accessToken, refreshToken);
    }
}
