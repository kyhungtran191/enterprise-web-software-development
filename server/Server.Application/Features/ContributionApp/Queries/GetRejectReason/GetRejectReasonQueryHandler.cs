using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;

namespace Server.Application.Features.ContributionApp.Queries.GetRejectReason
{
    public class GetRejectReasonQueryHandler : IRequestHandler<GetRejectReasonQuery, ErrorOr<IResponseWrapper<string>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetRejectReasonQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper<string>>> Handle(GetRejectReasonQuery request, CancellationToken cancellationToken)
        {
            var contribution = await _unitOfWork.ContributionRepository.GetByIdAsync(request.Id);
            if (contribution == null)
            {
                return Errors.Contribution.NotFound;
            }

            if (contribution.Status != ContributionStatus.Reject)
            {
                return Errors.Contribution.NotRejected;
            }

            var data =  await _unitOfWork.ContributionRepository.GetRejectReason(contribution);
            return new ResponseWrapper<string>
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    $"Get reject reason of contribution {contribution.Id} successfully"
                },
                ResponseData = data
            };

        }
    }
}
