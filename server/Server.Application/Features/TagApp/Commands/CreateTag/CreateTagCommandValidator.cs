using FluentValidation;

namespace Server.Application.Features.TagApp.Commands.CreateTag
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(x=>x.TagName)
                .MaximumLength(256)
                .NotNull()
                .NotEmpty()
                .WithMessage("Tag Name must be not empty and the length must be less then 256");
        }

    }
}
