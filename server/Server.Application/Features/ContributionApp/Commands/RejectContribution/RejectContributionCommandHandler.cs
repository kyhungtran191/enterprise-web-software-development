using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;

namespace Server.Application.Features.ContributionApp.Commands.RejectContribution
{
    public class RejectContributionCommandHandler : IRequestHandler<RejectContributionCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RejectContributionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
