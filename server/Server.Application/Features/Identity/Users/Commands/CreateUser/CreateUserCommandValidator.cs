using FluentValidation;

namespace Server.Application.Features.Identity.Users.Commands.CreateUser;

//     public string Email { get; set; } = default!;
//     public string UserName { get; set; } = default!;
//     public string Password { get; set; } = default!;
//     public string Faculty { get; set; } = default!;
public class CreateUserCommandValidator : UserCommandValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .MaximumLength(255)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.UserName)
            .MaximumLength(255)
            .NotEmpty();

        RuleFor(x => x.Password)            
           .MaximumLength(255)
           .NotEmpty();

        RuleFor(x => x.Faculty)
            .NotEmpty();
    }
}