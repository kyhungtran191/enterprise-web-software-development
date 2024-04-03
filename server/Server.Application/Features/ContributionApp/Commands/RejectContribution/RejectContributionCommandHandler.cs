using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Contracts.Common;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.ContributionApp.Commands.RejectContribution
{
    public class RejectContributionCommandHandler : IRequestHandler<RejectContributionCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        public RejectContributionCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userManager = userManager;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(RejectContributionCommand request, CancellationToken cancellationToken)
        {
            var contribution = await _unitOfWork.ContributionRepository.GetByIdAsync(request.Id);
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
            if (contribution.Status == ContributionStatus.Reject)
            {
                return Errors.Contribution.AlreadyRejected;
            }

            if (contribution.Status == ContributionStatus.Approve)
            {
                return Errors.Contribution.AlreadyApproved;
            }

            await _unitOfWork.ContributionRepository.Reject(contribution, request.UserId, request.Note);
            var student = await _userManager.FindByIdAsync(contribution.UserId.ToString());
            _emailService.SendEmail(new MailRequest
            {
                ToEmail = student.Email,
                Body = $"Coordinator reject your contribution with reason: {request.Note}",
                Subject = "REJECTED CONTRIBUTION"
            });
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    "Reject contribution successfully"
                }
            };
        }
    }
}
