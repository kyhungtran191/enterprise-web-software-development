namespace Server.Application.Features.Identity.Users.Commands.CreateUser;

public class CreateUserCommand : UserCommandBase
{
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Faculty { get; set; } = default!;
}