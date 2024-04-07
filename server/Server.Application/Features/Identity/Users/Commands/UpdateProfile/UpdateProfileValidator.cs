using FluentValidation;

namespace Server.Application.Features.Identity.Users.Commands.UpdateProfile
{
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileValidator()
        {
            //RuleFor(x => x.Email)
            //    .MaximumLength(255)
            //    .EmailAddress()
            //    .NotEmpty()
            //    .WithMessage("Please enter an valid email");

            RuleFor(x => x.FirstName)
                .MaximumLength(255)
                .NotEmpty()
                .WithMessage("First name is required");
            RuleFor(x => x.LastName)
                .MaximumLength(255)
                .NotEmpty()
                .WithMessage("Last name is required");
            //RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(50).WithMessage("Please enter phone number");


        }
    }
}
