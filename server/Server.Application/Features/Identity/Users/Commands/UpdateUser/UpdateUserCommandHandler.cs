using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Constants;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand, ErrorOr<IResponseWrapper>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IFacultyRepository _facultyRepository;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public UpdateUserCommandHandler(UserManager<AppUser> userManager, IMapper mapper, RoleManager<AppRole> roleManager, IFacultyRepository facultyRepository, IMediaService mediaService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
        _facultyRepository = facultyRepository;
        _mediaService = mediaService;
    }

    public async Task<ErrorOr<IResponseWrapper>> Handle(UpdateUserCommand request,
                                                 CancellationToken cancellationToken)
    {
        var userFromDb = await _userManager.FindByIdAsync(request.Id.ToString());

        if (userFromDb is null)
        {
            return Errors.User.CannotFound;
        }

        var newRoleFromDb = await _roleManager.FindByIdAsync(request.RoleId.ToString());

        if (newRoleFromDb is null)
        {
            return Errors.Roles.NotFound;
        }

        var newFacultyFromDb = await _facultyRepository.GetByIdAsync(request.FacultyId);

        if (newFacultyFromDb is null)
        {
            return Errors.Faculty.CannotFound;
        }

        // remove old roles
        var currentRoles = await _userManager.GetRolesAsync(userFromDb);
        var roleRemovedResult = await _userManager.RemoveFromRolesAsync(userFromDb, currentRoles);

        if (!roleRemovedResult.Succeeded)
        {
            return roleRemovedResult.GetIdentityResultErrorDescriptions();
        }

        // update new role
        await _userManager.AddToRoleAsync(userFromDb, newRoleFromDb.Name!);        

        _mapper.Map(request, userFromDb);

        userFromDb.FacultyId = newFacultyFromDb.Id;
        // update avatar
        if (request.Avatar is not null)
        {
            // remove
            //var existingFiles = userFromDb.AvatarPublicId;
            //if (existingFiles is not null)
            //{
            //    var removeFilePaths = new List<string>();
            //    var removeFileTypes = new List<string>();


            //    removeFilePaths.Add(existingFiles);
            //    removeFileTypes.Add("Image");



            //    await _mediaService.RemoveFromCloudinary(removeFilePaths, removeFileTypes);
            //}

            // update
            var newAvatarList = new List<IFormFile>
            {
                request.Avatar,
            };
            var infos = await _mediaService.UploadFileCloudinary(newAvatarList, FileType.Avatar, userFromDb.Id);
            foreach (var info in infos)
            {
                userFromDb.Avatar = info.Path;
                userFromDb.AvatarPublicId = info.PublicId;
            }

        }

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