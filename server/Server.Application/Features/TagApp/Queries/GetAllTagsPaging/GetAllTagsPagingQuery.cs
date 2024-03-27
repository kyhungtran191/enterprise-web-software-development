using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Features.TagApp.Queries.GetAllTagsPaging
{
    public class GetAllTagsPagingQuery : PagingDto, IRequest<ErrorOr<IResponseWrapper<PagedResult<TagDto>>>>
    {
    }
}
