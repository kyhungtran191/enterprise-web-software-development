using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Contracts.Common;
using Server.Contracts.Contributions;
using Server.Domain.Common.Constants;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;
using static Server.Domain.Common.Errors.Errors;
using File = Server.Domain.Entity.Content.File;


namespace Server.Application.Features.ContributionApp.Commands.UpdateContribution
{
    public class UpdateContributionCommandHandler : IRequestHandler<UpdateContributionCommand,ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMediaService _mediaService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailService _emailService;
        public UpdateContributionCommandHandler(IUnitOfWork unitOfWork,IMapper mapper, IDateTimeProvider dateTimeProvider, IMediaService mediaService, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _mediaService = mediaService;
            _roleManager = roleManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<ErrorOr<IResponseWrapper>> Handle(UpdateContributionCommand request, CancellationToken cancellationToken)
        {
            var itemFromDb  =  await _unitOfWork.ContributionRepository.GetByIdAsync(request.ContributionId);
            if(itemFromDb is null)
            {
                return Errors.Contribution.NotFound;
                
            }

            if (!request.IsConfirmed && !itemFromDb.IsConfirmed)
            {
                return Errors.Contribution.NotConfirmed;
            }
            if (itemFromDb.DateDeleted.HasValue)
            {
                return Errors.Contribution.Deleted;
            }

            if (itemFromDb.Status == ContributionStatus.Approve)
            {
                return Errors.Contribution.AlreadyApproved;
            }
            _mapper.Map(request,itemFromDb);
            itemFromDb.DateEdited = _dateTimeProvider.UtcNow;
            await _unitOfWork.CompleteAsync();
            var filePublicId = new List<string>();
            var fileTypes = new List<string>();
            var thumbnailPublicId = new List<string>();
            var thumbnailTypes = new List<string>();
            if (request.Thumbnail is not null || request.Files.Count > 0)
            {
                // remove old files
                var existingFiles = await _unitOfWork.FileRepository.GetByContribution(itemFromDb);
                if (existingFiles.Count == 0)
                {
                    return Errors.Contribution.NoFilesFound;
                }
                //var removeFilePaths = new List<string>();
                //var removeFileTypes = new List<string>();
                foreach (var existingFile in existingFiles)
                {
                    _unitOfWork.FileRepository.Remove(existingFile);
                    //removeFilePaths.Add(existingFile.PublicId);
                    //removeFileTypes.Add(existingFile.Type);

                }

                //await _mediaService.RemoveFromCloudinary(removeFilePaths,removeFileTypes);

                // create new files
                var thumbnailList = new List<IFormFile>
                {
                    request.Thumbnail
                };

                var thumbnailInfo = await _mediaService.UploadFileCloudinary(thumbnailList, FileType.Thumbnail,itemFromDb.Id);
                var fileInfo = await _mediaService.UploadFileCloudinary(request.Files, FileType.File, itemFromDb.Id);
                filePublicId = fileInfo.Select(x => x.PublicId).ToList();
                fileTypes = fileInfo.Select(x => x.Type).ToList();
                thumbnailPublicId = thumbnailInfo.Select(x => x.PublicId).ToList();
                thumbnailTypes = thumbnailInfo.Select(x => x.Type).ToList();
                foreach (var info in fileInfo.Concat(thumbnailInfo))
                {
                    _unitOfWork.FileRepository.Add(new File
                    {
                        ContributionId = itemFromDb.Id,
                        Path = info.Path,
                        Type = info.Type,
                        Name = info.Name,
                        PublicId = info.PublicId,
                        Extension = info.Extension,
                        DateEdited = _dateTimeProvider.UtcNow
                    });
                }
                await _unitOfWork.CompleteAsync();
            }

            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                await _mediaService.RemoveFromCloudinary(filePublicId, fileTypes);
                await _mediaService.RemoveFromCloudinary(thumbnailPublicId, thumbnailTypes);
                return Errors.User.CannotFound;
            }
            // add email service later
            var coordinator = await _userManager.FindByFacultyIdAsync(_roleManager, (Guid)user.FacultyId!);
            var faculty = await _unitOfWork.FacultyRepository.GetByIdAsync((Guid)user.FacultyId);
            _emailService.SendEmail(new MailRequest
            {
                ToEmail = coordinator.Email,
                Body= $"<div style=\"font-family: Arial, sans-serif; color: #800080; padding: 20px;\">\r\n " +
                $" <h2>Blog Edition request are pending</h2>\r\n " +
                $" <p style=\"margin: 5px 0; font-size: 18px;\">Blog Title: Web development 2</p>\r\n " +
                $" <p style=\"margin: 5px 0; font-size: 18px;\">Content: Development</p>\r\n" +
                $"  <p style=\"margin: 5px 0; font-size: 18px;\">User: {user.UserName}</p>\r\n " +
                $"  <p style=\"margin: 5px 0; font-size: 18px;\">Faculty: {faculty.Name}</p>\r\n " +
                $" <p style=\"margin: 5px 0; font-size: 18px;\">Academic Year: 2024-2025</p>\r\n</div>",
                Subject = "Edit CONTRIBUTION"
            });

            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    "Update contribution successfully!"
                }
            };
            
        }
    }
}
