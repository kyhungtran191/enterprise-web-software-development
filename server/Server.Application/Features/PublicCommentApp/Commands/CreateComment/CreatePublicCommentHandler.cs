using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Features.CommentApp.Commands;
using Server.Application.Wrappers;
using Server.Domain.Common.Constants;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.Application.Features.PublicCommentApp.Commands.CreateComment
{
    public class CreatePublicCommentHandler : IRequestHandler<CreatePublicCommentCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public CreatePublicCommentHandler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(CreatePublicCommentCommand request, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.PublicContributionRepository.GetByIdAsync(request.ContributionId);
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
            //if (roles.Contains(Roles.Student))
            //{
            //    if (user.Id != item.UserId)
            //    {
            //        return Errors.Contribution.NotBelong;
            //    }
            //}

            _unitOfWork.PublicCommentRepository.Add(new ContributionPublicComment
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
