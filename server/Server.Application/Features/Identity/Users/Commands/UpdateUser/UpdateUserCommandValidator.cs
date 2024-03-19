using FluentValidation;

namespace Server.Application.Features.Identity.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : UserCommandValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}