using FluentValidation;

namespace Server.Application.Features.Authentication;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(255)
            .NotNull()
            .NotEmpty()
            .WithMessage("Email is not empty and the length must be less than 255");
        
        RuleFor(x => x.Password)
            .MaximumLength(255)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password is not empty and the length must be less than 255");
    }
}