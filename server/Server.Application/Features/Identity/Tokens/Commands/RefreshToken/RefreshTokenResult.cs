namespace Server.Application.Features.Identity.Tokens.Commands.RefreshToken;

public class RefreshTokenResult
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}