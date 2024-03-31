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

namespace Server.Application.Features.Identity.Users.Commands.CreateUser;

public class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, ErrorOr<IResponseWrapper>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public CreateUserCommandHandler(UserManager<AppUser> userManager,
                                    IMapper mapper,
                                    IUnitOfWork unitOfWork, IMediaService mediaService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _mediaService = mediaService;
    }

    public async Task<ErrorOr<IResponseWrapper>> Handle(CreateUserCommand request,
                                                    CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        var facultyFromDb =
            await _unitOfWork
            .FacultyRepository
            .GetByIdAsync(request.FacultyId);

        if (facultyFromDb is null)
        {
            return Errors.Faculty.CannotFound;
        }
        var newUser = new AppUser();

        _mapper.Map(request, newUser);
        newUser.Id = Guid.NewGuid();
        newUser.FacultyId = facultyFromDb.Id;
        newUser.PasswordHash = new PasswordHasher<AppUser>().HashPassword(newUser, request.Password);
        // avatar
        if (request.Avatar is not null)
        {
            var avatarList = new List<IFormFile>();
            avatarList.Add(request.Avatar);
            var avatarInfo = await _mediaService.UploadFileCloudinary(avatarList, FileType.Avatar,newUser.Id);
            foreach (var info in avatarInfo)
            {
                newUser.Avatar = info.Path;
                newUser.AvatarPublicId = info.PublicId;
            }
        }
     

        var result = await _userManager.CreateAsync(newUser);

        if (!result.Succeeded)
        {
            return result.GetIdentityResultErrorDescriptions();
        }

        return new ResponseWrapper
        {
            IsSuccessfull = true,
            Messages = new List<string>
                {
                    "Create new user successfully!"
                }
        };
    }
}