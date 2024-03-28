using AutoMapper;
using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.ContributionApp.Queries.GetContributionByTitle
{
    public class GetContributionBySlugQueryHandler : IRequestHandler<GetContributionBySlugQuery, ErrorOr<IResponseWrapper<ContributionDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetContributionBySlugQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ErrorOr<IResponseWrapper<ContributionDto>>> Handle(GetContributionBySlugQuery request, CancellationToken cancellationToken)
        {
            var contributionFromDb = await _unitOfWork.ContributionRepository.GetContributionBySlug(request.Slug);
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
