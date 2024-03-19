using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Extensions;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand, ErrorOr<IResponseWrapper>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ErrorOr<IResponseWrapper>> Handle(UpdateUserCommand request,
                                                 CancellationToken cancellationToken)
    {
        var userFromDb = await _userManager.FindByIdAsync(request.Id.ToString());

        if (userFromDb is null)
        {
            return Errors.User.CannotFound;
        }

        _mapper.Map(request, userFromDb);

        var result = await _userManager.UpdateAsync(userFromDb);

        if (!result.Succeeded)
        {
            return result.GetIdentityResultErrorDescriptions();
        }

        return new ResponseWrapper
        {
            IsSuccessfull = true,
            Messages = new List<string> 
            { 
                "Update user successfully!"
            }
        };
    }
}