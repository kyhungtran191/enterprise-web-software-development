using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Features.PublicContributionApp.Queries.GetAllPublicContributionPaging
{
    public class GetAllPublicContributionPagingHandler : IRequestHandler<GetAllPublicContributionPagingQuery, ErrorOr<IResponseWrapper<PagedResult<PublicContributionInListDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllPublicContributionPagingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public async Task<ErrorOr<IResponseWrapper<PagedResult<PublicContributionInListDto>>>> Handle(GetAllPublicContributionPagingQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.PublicContributionRepository.GetAllPaging(request.Keyword, request.Year, request.FacultyName, request.Status, request.PageIndex, request.PageSize);
            return new ResponseWrapper<PagedResult<PublicContributionInListDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    $"Get all public contribution success"
                },
                ResponseData = items
            };
        }
    }
}
