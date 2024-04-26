using System.Collections.Generic;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Server.Application.Common.Dtos.Announcement;
using Server.Application.Common.Interfaces.Hubs.Announcement;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Constants;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.CommentApp.Commands
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<AnnouncementHub> _announcementHub;
        private readonly IAnnouncementService _announcementService;
        private readonly ICurrentUserService _currentUserService;
        public CreateCommentHandler(IUnitOfWork unitOfWork,
                                    UserManager<AppUser> userManager,
                                    IHubContext<AnnouncementHub> announcementHub,
                                    IAnnouncementService announcementService,
                                    ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _announcementHub = announcementHub;
            _announcementService = announcementService;
            _currentUserService = currentUserService;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId.ToString());
             if (currentUser is null)
            {
                return Errors.CurrentUser.CannotFound;
            }

            var item = await _unitOfWork.ContributionRepository.GetByIdAsync(request.ContributionId);
            if (item is null)
            {
                return Errors.Contribution.NotFound;
            }
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
            {
                return Errors.User.CannotFound;
            }
            
            var roles = await _userManager.GetRolesAsync(user);

            bool isStudent = false;

            if (roles.Contains(Roles.Student))
            {
                isStudent = true;
                if (user.Id != item.UserId)
                {
                    return Errors.Contribution.NotBelong;
                }
            }

            _unitOfWork.CommentRepository.Add(new ContributionComment
            {
                ContributionId = request.ContributionId,
                UserId = request.UserId,
                Content = request.Content,
            });
            item.IsCoordinatorComment = true;
            await _unitOfWork.CompleteAsync();

            // notify
            var notificationId = Guid.NewGuid().ToString();
            var announcementDto = new AnnouncementDto()
            {
                Id = notificationId,
                Title = "New Comment of Contribution",
                DateCreated = DateTime.Now,
                Content = $"Contribution has been commented",
                UserId = currentUser.Id,
                Type = "Contribution-Private-Comment",
                Username = currentUser.UserName,
                Avatar = currentUser.Avatar,
                Slug = item.Slug
            };
            _announcementService.Add(announcementDto);

            var receiverId = isStudent ? request.UserId : item.UserId;

            var announcementUserDto = new AnnouncementUserDto
            {
                AnnouncementId = notificationId,
                HasRead = false,
                UserId = receiverId,
            };
            _announcementService.AddToAnnouncementUsers(new List<AnnouncementUserDto> {
                announcementUserDto
            });

            await _unitOfWork.CompleteAsync();

            await _announcementHub
                .Clients
                .Users(receiverId.ToString())
                .SendAsync("GetNewCommentAnnouncement", announcementDto);

            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Comment success" }
            };
        }
    }
}
