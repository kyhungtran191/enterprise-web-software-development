using FluentValidation;

namespace Server.Application.Features.Identity.Users.Commands.CreateGuest
{
    public class CreateGuestValidator :AbstractValidator<CreateGuestCommand>
    {
        public CreateGuestValidator()
        {
            RuleFor(x => x.Email)
                .MaximumLength(255)
                .EmailAddress()
                .NotEmpty();

            RuleFor(x => x.UserName)
                .MaximumLength(255)
                .NotEmpty();
        }
    }
}
