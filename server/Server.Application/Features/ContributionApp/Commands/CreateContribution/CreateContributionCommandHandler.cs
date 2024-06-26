﻿using System.Collections.Immutable;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Server.Application.Common.Dtos.Announcement;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Hubs.Announcement;
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
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IHubContext<AnnouncementHub> _announcementHub;
        private readonly IAnnouncementService _announcementService;

        public CreateContributionCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IEmailService emailService, IMediaService mediaService, IHubContext<AnnouncementHub> announcementHub, IAnnouncementService announcementService)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _emailService = emailService;
            _userManager = userManager;
            _mediaService = mediaService;
            _roleManager = roleManager;
            _announcementHub = announcementHub;
            _announcementService = announcementService;
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

            if (!(await _unitOfWork.AcademicYearRepository.CanSubmitAsync(_dateTimeProvider.UtcNow)))
            {
                return Errors.Contribution.CannotSubmit;
            }
            var academicYear = await _unitOfWork.AcademicYearRepository.GetAcademicYearByDateAsync(_dateTimeProvider.UtcNow);
            if (academicYear is null)
            {
                return Errors.Contribution.AcademicYearNotFound;
            }
            var contributon = new Contribution
            {
                Id = Guid.NewGuid(),
                AcademicYearId = academicYear.Id,
                Title = request.Title,
                Slug = request.Slug,
                FacultyId = request.FacultyId,
                UserId = request.UserId,
                IsConfirmed = true,
                SubmissionDate = _dateTimeProvider.UtcNow,
                Content = request.Content,
                ShortDescription = request.ShortDescription
            };
            _unitOfWork.ContributionRepository.Add(contributon);
            var thumbnailList = new List<IFormFile>();
            //var filePath = new List<string>();
            //var thumbnailPath = new List<string>();
            var filePublicId = new List<string>();
            var fileTypes = new List<string>();
            var thumbnailPublicId = new List<string>();
            var thumbnailTypes = new List<string>();

            if (request.Thumbnail is not null || request.Files.Count > 0)
            {

                thumbnailList.Add(request.Thumbnail);
                //var thumbnailInfo = await _mediaService.UploadFiles(thumbnailList, FileType.Thumbnail);
                //var fileInfo = await _mediaService.UploadFiles(request.Files, FileType.File);
                var thumbnailInfo = await _mediaService.UploadFileCloudinary(thumbnailList, FileType.Thumbnail, contributon.Id);
                var fileInfo = await _mediaService.UploadFileCloudinary(request.Files, FileType.File, contributon.Id);
                //filePath = fileInfo.Select(x => x.Path).ToList();
                //thumbnailPath = thumbnailInfo.Select(x => x.Path).ToList();
                filePublicId = fileInfo.Select(x => x.PublicId).ToList();
                fileTypes = fileInfo.Select(x => x.Type).ToList();
                thumbnailPublicId = thumbnailInfo.Select(x => x.PublicId).ToList();
                thumbnailTypes = thumbnailInfo.Select(x => x.Type).ToList();
                foreach (var info in fileInfo.Concat(thumbnailInfo))
                {
                    _unitOfWork.FileRepository.Add(new File
                    {
                        ContributionId = contributon.Id,
                        Path = info.Path,
                        Type = info.Type,
                        Name = info.Name,
                        PublicId = info.PublicId,
                        Extension = info.Extension,
                    });
                }
            }

            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                await _mediaService.RemoveFromCloudinary(filePublicId, fileTypes);
                await _mediaService.RemoveFromCloudinary(thumbnailPublicId, thumbnailTypes);
                return Errors.User.CannotFound;
            }

            var coordinators = await _userManager.FindByFacultyIdAsync(_roleManager, (Guid)user.FacultyId!);
            var faculty = await _unitOfWork.FacultyRepository.GetByIdAsync((Guid)user.FacultyId);

            foreach (var coordinator in coordinators)
            {
                _emailService.SendEmail(new MailRequest
                {
                    ToEmail = coordinator.Email,
                    Body = $"<div style=\"font-family: Arial, sans-serif; color: #800080; padding: 20px;\">\r\n " +
                           $" <h2>New Blog request are pending</h2>\r\n " +
                           $" <p style=\"margin: 5px 0; font-size: 18px;\">Blog Title: Web development 2</p>\r\n " +
                           $" <p style=\"margin: 5px 0; font-size: 18px;\">Content: Development</p>\r\n" +
                           $"  <p style=\"margin: 5px 0; font-size: 18px;\">User: {user.UserName}</p>\r\n " +
                           $"  <p style=\"margin: 5px 0; font-size: 18px;\">Faculty: {faculty.Name}</p>\r\n " +
                           $" <p style=\"margin: 5px 0; font-size: 18px;\">Academic Year: 2024-2025</p>\r\n</div>",
                    Subject = "NEW CONTRIBUTION"
                });
                await _unitOfWork.CompleteAsync();
                // send to approve 
                await _unitOfWork.ContributionRepository.SendToApprove(contributon.Id, user.Id);
                await _unitOfWork.CompleteAsync();


            }

            // notify
            var notificationId = Guid.NewGuid().ToString();
            var announcementDto = new AnnouncementDto()
            {
                Id = notificationId,
                Title = "Contribution created",
                DateCreated = DateTime.Now,
                Content = $"Contribution has been created",
                UserId = user.Id,
                Type = "Contribution-CreateContribution",
                Username = user.UserName,
                Avatar = user.Avatar,
                Slug = contributon.Slug
            };
            _announcementService.Add(announcementDto);

            var announcementUsers = coordinators.Select(coordinator => new AnnouncementUserDto
            {
                AnnouncementId = notificationId,
                HasRead = false,
                UserId = coordinator.Id
            });
            _announcementService.AddToAnnouncementUsers(announcementUsers);

            await _unitOfWork.CompleteAsync();

            await _announcementHub
                .Clients
                .Users(coordinators.Select(x => x.Id.ToString()).ToImmutableList())
                .SendAsync("GetNewAnnouncement", announcementDto);

            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Create contribution successfully!" }
            };
        }
    }
}

