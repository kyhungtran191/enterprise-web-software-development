using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Contracts.Common;
using Server.Contracts.Contributions;
using Server.Domain.Common.Constants;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;
using File = Server.Domain.Entity.Content.File;

namespace Server.Application.Features.ContributionApp.Commands.CreateContribution
{
    public class CreateContributionCommandHandler : IRequestHandler<CreateContributionCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IMediaService _mediaService;
        private readonly UserManager<AppUser> _userManager;

        public CreateContributionCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, UserManager<AppUser> userManager, IEmailService emailService,IMediaService mediaService) 
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _emailService = emailService;
            _userManager = userManager;
            _mediaService = mediaService;
        }

        public async Task<ErrorOr<IResponseWrapper>> Handle(CreateContributionCommand request,
            CancellationToken cancellationToken)
        {
            if (await _unitOfWork.ContributionRepository.IsSlugAlreadyExisted(request.Slug))
            {
                return Errors.Contribution.SlugExist;
            }
            if (!request.IsConfirmed)
            {
                return Errors.Contribution.NotConfirmed;
            }
            if (await _unitOfWork.ContributionRepository.GetContributionBySlug(request.Slug) is not null)
            {
                return Errors.Contribution.AlreadyExist;
            }

            if (await _unitOfWork.AcademicYearRepository.GetByIdAsync(request.AcademicYearId) is null)
            {
                return Errors.Contribution.AcademicYearNotFound;
            }

            var contributon = new Contribution
            {
                AcademicYearId = request.AcademicYearId,
                Title = request.Title,
                Slug = request.Slug,
                FacultyId = request.FacultyId,
                UserId = request.UserId,
                IsConfirmed = true,
                SubmissionDate = _dateTimeProvider.UtcNow

            };
            _unitOfWork.ContributionRepository.Add(contributon);

            await _unitOfWork.CompleteAsync();
            if (request.Thumbnail  is not null || request.Files.Count > 0)
            {
                var thumbnailList = new List<IFormFile>();

                if (request.Thumbnail != null)
                {
                    thumbnailList.Add(request.Thumbnail);
                }


                var thumbnailInfo = await _mediaService.UploadFiles(thumbnailList, FileType.Thumbnail);
                var fileInfo = await _mediaService.UploadFiles(request.Files, FileType.File);

                foreach (var info in fileInfo.Concat(thumbnailInfo))
                {
                    _unitOfWork.FileRepository.Add(new File
                    {
                        ContributionId = contributon.Id,
                        Path = info.Path,
                        Type = info.Type,
                        Name = info.Name,
                    });
                }
            }
           
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Errors.User.CannotFound;
            }

             _emailService.SendEmail(new MailRequest
            {
                ToEmail = "nguahoang2003@gmail.com",
                Body = $"User with Id {user.Id} submit new contribution",
                Subject = "NEW CONTRIBUTION"
            });
            // send to approve 
            await _unitOfWork.ContributionRepository.SendToApprove(contributon.Id, user.Id);
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Create contribution successfully!" }
            };

        }
    }
}

