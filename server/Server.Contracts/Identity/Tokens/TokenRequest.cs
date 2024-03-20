namespace Server.Contracts.Identity.Tokens;

public record TokenRequest(
    string AccessToken,
    string RefreshToken
);