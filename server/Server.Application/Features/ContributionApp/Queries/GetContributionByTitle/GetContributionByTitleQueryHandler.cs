using AutoMapper;
using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.ContributionApp.Queries.GetContributionByTitle
{
    public class GetContributionByTitleQueryHandler : IRequestHandler<GetContributionByTitleQuery, ErrorOr<IResponseWrapper<ContributionDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetContributionByTitleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ErrorOr<IResponseWrapper<ContributionDto>>> Handle(GetContributionByTitleQuery request, CancellationToken cancellationToken)
        {
            var contributionFromDb = await _unitOfWork.ContributionRepository.GetContributionByTitle(request.Title);
            if (contributionFromDb == null)
            {
                return Errors.Contribution.NotFound;
            }

            return new ResponseWrapper<ContributionDto>
            {
                IsSuccessfull = true,
                Messages = new List<string> { "Get contribution by slug successfully!" },
                ResponseData = contributionFromDb
            };
        }
    }
}
