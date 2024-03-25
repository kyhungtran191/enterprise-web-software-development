using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Features.ContributionApp.Queries.GetAllContributionsPaging
{
    public class GetAllContributionsQueryHandler : IRequestHandler<GetAllContributionsPagingQuery, ErrorOr<IResponseWrapper<PagedResult<ContributionInListDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllContributionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper<PagedResult<ContributionInListDto>>>> Handle(GetAllContributionsPagingQuery request, CancellationToken cancellationToken)
        {
            var contributions =
                await _unitOfWork.ContributionRepository.GetAllPaging(request.Keyword,request.AcademicYearId,request.PageIndex,request.PageSize);
            return new ResponseWrapper<PagedResult<ContributionInListDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    $"Get All Paging Contributions Successfully"
                },
                ResponseData = contributions
            };
        }
    }
}
