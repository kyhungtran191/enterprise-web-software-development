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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.Application.Features.Identity.Users.Commands.UpdateProfile
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProfileHandler(UserManager<AppUser> userManager,IMapper mapper,IMediaService mediaService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _mediaService = mediaService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var userFromDb = await _userManager.FindByIdAsync(request.UserId.ToString());
           

            if (userFromDb is null)
            {
                return Errors.User.CannotFound;
            }

           
            userFromDb.FirstName = request.FirstName;
            userFromDb.LastName = request.LastName;
            userFromDb.Email = request.Email;
            userFromDb.PhoneNumber = request.PhoneNumber;
            userFromDb.Dob = request.Dob;
          
            // update avatar
            if (request.Avatar is not null)
            {
                // remove
                var existingFiles = userFromDb.Avatar;
                if (existingFiles is null)
                {
                    return Errors.User.NoAvatarFound;
                }

                var removeFilePaths = new List<string>
                {
                    existingFiles
                };
                await _mediaService.RemoveFile(removeFilePaths);
                // update
                var newAvatarList = new List<IFormFile>
                {
                    request.Avatar,
                };
                var infos = await _mediaService.UploadFiles(newAvatarList, FileType.Avatar);
                foreach (var info in infos)
                {
                    userFromDb.Avatar = info.Path;
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
                    "Update user profile successfully!"
                }
            };
        }
    }
}
