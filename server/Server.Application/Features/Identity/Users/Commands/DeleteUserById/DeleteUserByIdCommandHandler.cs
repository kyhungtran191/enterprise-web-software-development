using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Extensions;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Users.Commands.DeleteUserById;

public class DeleteUserByIdCommandHandler
    : IRequestHandler<DeleteUserByIdCommand, ErrorOr<IResponseWrapper>>
{
    private readonly UserManager<AppUser> _userManager;

    public DeleteUserByIdCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ErrorOr<IResponseWrapper>> Handle(DeleteUserByIdCommand request, 
                                                        CancellationToken cancellationToken)
    {

        var userFromDb = await _userManager.FindByIdAsync(request.Id.ToString());

        if (userFromDb is null)
        {
            return Errors.User.CannotFound;
        }

        var result = await _userManager.DeleteAsync(userFromDb);

        if (!result.Succeeded)
        {
            return result.GetIdentityResultErrorDescriptions();
        }

        return new ResponseWrapper
        {
            IsSuccessfull = true,
            Messages = new List<string> { $"Delete user ({userFromDb.Email}) successfully! " },
        };
    }
}