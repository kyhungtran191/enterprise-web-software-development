using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.ContributionApp.Queries.GetActivityLog
{
    internal class GetActivityLogQueryHandler : IRequestHandler<GetActivityLogQuery, ErrorOr<IResponseWrapper<List<ActivityLogDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetActivityLogQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public async Task<ErrorOr<IResponseWrapper<List<ActivityLogDto>>>> Handle(GetActivityLogQuery request, CancellationToken cancellationToken)
        {
            var contribution = await _unitOfWork.ContributionRepository.GetByIdAsync(request.ContributionId);
            if (contribution is null)
            {
                return Errors.Contribution.NotFound;
            }

            var data = await _unitOfWork.ContributionRepository.GetActivityLogs(contribution);
            return new ResponseWrapper<List<ActivityLogDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    $"Get activity logs of contribution ${contribution.Id} successfully"
                },
                ResponseData = data

            };
        }
    }
}
