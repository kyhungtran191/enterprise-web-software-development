using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Features.ContributionApp.Commands.ApproveContributions;
using Server.Application.Wrappers;
using Server.Contracts.Common;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;

namespace Server.Application.Features.ContributionApp.Commands.ApproveContributions
{
    public class ApproveContributionsCommandHandler : IRequestHandler<ApproveContributionsCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        public ApproveContributionsCommandHandler(IUnitOfWork unitOfWork,IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
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
