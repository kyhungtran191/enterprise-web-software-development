using FluentValidation;

namespace Server.Application.Features.CommentApp.Commands
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Can not create empty comment");
        }
    }
}
