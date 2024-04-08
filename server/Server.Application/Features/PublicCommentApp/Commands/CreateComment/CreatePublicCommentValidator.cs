using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Server.Application.Features.PublicCommentApp.Commands.CreateComment
{
    public class CreatePublicCommentValidator : AbstractValidator<CreatePublicCommentCommand>
    {
        public CreatePublicCommentValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Can not create empty comment");
        }
    }
}
