using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.PublicContributionApp.Queries.GetTopContribution
{
    public class GetFeaturedContributionHandler : IRequestHandler<GetFeaturedContributionQuery, ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetFeaturedContributionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>> Handle(GetFeaturedContributionQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PublicContributionRepository.GetFeaturedContribution();
            if (result is null)
            {
                return Errors.Contribution.NotFoundPublic;
            }

            return new ResponseWrapper<List<PublicContributionInListDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get Featured Contribution Success" },
                ResponseData = result
            };
        }
    }
}
