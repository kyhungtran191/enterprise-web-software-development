namespace Server.Application.Features.Authentication;

public record LoginResult(
    string AccessToken,
    string RefreshToken
);
