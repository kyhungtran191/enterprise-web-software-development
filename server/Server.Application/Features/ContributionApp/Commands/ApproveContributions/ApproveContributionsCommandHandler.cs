using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<AppUser> _userManager;
        public ApproveContributionsCommandHandler(IUnitOfWork unitOfWork,IEmailService emailService, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userManager = userManager;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(ApproveContributionsCommand request, CancellationToken cancellationToken)
        {

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

               
                await _unitOfWork.ContributionRepository.Approve(contribution,request.UserId);
                var student = await _userManager.FindByIdAsync(contribution.UserId.ToString());
                var faculty = await _unitOfWork.FacultyRepository.GetByIdAsync((Guid)student?.FacultyId!);
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
