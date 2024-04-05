using System.Collections.Generic;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Interfaces.Persistence;
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
        public CreateCommentHandler(IUnitOfWork unitOfWork,UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.ContributionRepository.GetByIdAsync(request.ContributionId);
            if(item is null)
            {
                return Errors.Contribution.NotFound;
            }
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
            {
                return Errors.User.CannotFound;
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(Roles.Student))
            {
                if(user.Id != item.UserId)
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
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Comment success" }
            };
        }
    }
}
