using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Queries.GetTopContributors
{
    public class GetTopContributorsHandler : IRequestHandler<GetTopContributorsQuery, ErrorOr<IResponseWrapper<List<TopContributorDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetTopContributorsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public async Task<ErrorOr<IResponseWrapper<List<TopContributorDto>>>> Handle(GetTopContributorsQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PublicContributionRepository.GetTopContributors();
            return new ResponseWrapper<List<TopContributorDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get list contributors successfully" },
                ResponseData = result
            };
        }
    }
}
