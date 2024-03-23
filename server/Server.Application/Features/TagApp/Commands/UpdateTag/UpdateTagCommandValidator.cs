using FluentValidation;

namespace Server.Application.Features.TagApp.Commands.UpdateTag
{
    public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagCommandValidator()
        {
            RuleFor(x => x.TagName)
                .MaximumLength(256)
                .NotNull()
                .NotEmpty()
                .WithMessage("Tag Name must be not empty and the length must be less then 256"); 
        }
    }
}
