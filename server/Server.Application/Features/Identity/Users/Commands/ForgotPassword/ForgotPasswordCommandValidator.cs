using FluentValidation;

namespace Server.Application.Features.Identity.Users.Commands.ForgotPassword
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x=>x.Email).NotNull().NotEmpty().EmailAddress().WithMessage("Enter a valid email");
        }
    }
}
