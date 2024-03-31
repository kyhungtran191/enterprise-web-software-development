using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.GetTop4Contributions
{
    public class GetTop4ContributionsHandler : IRequestHandler<GetTop4ContributionQuery, ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTop4ContributionsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>> Handle(GetTop4ContributionQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.PublicContributionRepository.GetTopContributions();
            return new ResponseWrapper<List<PublicContributionInListDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    $"Get contributions successfully"
                },
                ResponseData = items
            };
        }
    }
}
