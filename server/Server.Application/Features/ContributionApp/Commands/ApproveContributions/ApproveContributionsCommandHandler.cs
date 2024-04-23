using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos.Announcement;
using Server.Application.Common.Interfaces.Hubs.Announcement;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Features.ContributionApp.Commands.ApproveContributions;
using Server.Application.Wrappers;
using Server.Contracts.Common;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;
using static Server.Domain.Common.Errors.Errors;

namespace Server.Application.Features.ContributionApp.Commands.ApproveContributions
{
    public class ApproveContributionsCommandHandler : IRequestHandler<ApproveContributionsCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IHubContext<AnnouncementHub> _announcementHub;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAnnouncementService _announcementService;
        public ApproveContributionsCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService, UserManager<AppUser> userManager, IHubContext<AnnouncementHub> announcementHub, IAnnouncementService announcementService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userManager = userManager;
            _announcementHub = announcementHub;
            _announcementService = announcementService;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(ApproveContributionsCommand request, CancellationToken cancellationToken)
        {
            List<Guid> studentIds = new();
            string coordinatorId = string.Empty;
            AppUser? coordinator = null;
            foreach (var id in request.Ids)
            {
                var contribution = await _unitOfWork.ContributionRepository.GetByIdAsync(id);
                if (contribution is null)
                {
                    return Errors.Contribution.NotFound;
                }

                if (contribution.DateDeleted.HasValue)
                {
                    return Errors.Contribution.Deleted;
                }

                if (!contribution.IsConfirmed)
                {
                    return Errors.Contribution.NotConfirmed;
                }
                if (contribution.Status == ContributionStatus.Approve)
                {
                    return Errors.Contribution.AlreadyApproved;
                }
                if (contribution.Status == ContributionStatus.Reject)
                {
                    return Errors.Contribution.AlreadyRejected;
                }

                var student = await _userManager.FindByIdAsync(contribution.UserId.ToString());
                coordinator = await _userManager.FindByIdAsync(request.UserId.ToString());

                if (student.FacultyId != coordinator.FacultyId)
                {
                    return Errors.Contribution.NotBelongFaculty;
                }

                studentIds.Add(student.Id);

                var faculty = await _unitOfWork.FacultyRepository.GetByIdAsync((Guid)student?.FacultyId!);
                await _unitOfWork.ContributionRepository.Approve(contribution, request.UserId);
                _emailService.SendEmail(new MailRequest
                {
                    ToEmail = student.Email,
                    Body = $"<div style=\"font-family: Arial, sans-serif; color: #800080; padding: 20px;\">\r\n " +
                           $" <h2>Your contribution is approved</h2>\r\n " +
                           $" <p style=\"margin: 5px 0; font-size: 18px;\">Blog Title: Web development 2</p>\r\n " +
                           $" <p style=\"margin: 5px 0; font-size: 18px;\">Content: Development</p>\r\n" +
                           $"  <p style=\"margin: 5px 0; font-size: 18px;\">User: {student.UserName}</p>\r\n " +
                           $"  <p style=\"margin: 5px 0; font-size: 18px;\">Faculty: {faculty.Name}</p>\r\n " +
                           $" <p style=\"margin: 5px 0; font-size: 18px;\">Academic Year: 2024-2025</p>\r\n</div>",
                    Subject = "APPROVED CONTRIBUTION"
                });
            }

            await _unitOfWork.CompleteAsync();

            var contribution1 = await _unitOfWork.ContributionRepository.GetByIdAsync(request.Ids[0]);

            // notify
            var notificationId = Guid.NewGuid().ToString();
            var announcementDto = new AnnouncementDto()
            {
                Id = notificationId,
                Title = "Contribution approved",
                DateCreated = DateTime.Now,
                Content = $"Contribution has been approved",
                UserId = coordinator!.Id,
                Type = "Contribution-Approve",
                Username = coordinator?.UserName,
                Avatar = coordinator?.Avatar,
                Slug = contribution1.Slug
            };
            _announcementService.Add(announcementDto);

            var announcementUsers = studentIds.Select(studentId => new AnnouncementUserDto
            {
                AnnouncementId = notificationId,
                HasRead = false,
                UserId = studentId
            });
            _announcementService.AddToAnnouncementUsers(announcementUsers);

            await _unitOfWork.CompleteAsync();

            await _announcementHub
                .Clients
                .Users(studentIds.Select(x => x.ToString()))
                .SendAsync("GetNewAnnouncement", announcementDto);

            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    "Approve contributions successfully!"
                }
            };
        }
    }
}
