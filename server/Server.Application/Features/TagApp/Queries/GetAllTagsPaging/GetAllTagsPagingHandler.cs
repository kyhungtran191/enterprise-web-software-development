using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Features.TagApp.Queries.GetAllTagsPaging
{
    public class GetAllTagsPagingHandler : IRequestHandler<GetAllTagsPagingQuery, ErrorOr<IResponseWrapper<PagedResult<TagDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllTagsPagingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper<PagedResult<TagDto>>>> Handle(GetAllTagsPagingQuery request, CancellationToken cancellationToken)
        {
            var tags = await _unitOfWork.TagRepository.GetAllTagsPaging(request.Keyword, request.PageIndex, request.PageSize);
            return new ResponseWrapper<PagedResult<TagDto>>
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    $"Get all Paging Tags success",

                },
                ResponseData = tags
            };
        }
    }
}
