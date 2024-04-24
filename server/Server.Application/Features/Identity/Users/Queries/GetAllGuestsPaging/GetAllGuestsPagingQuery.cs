using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos;
using Server.Application.Common.Dtos.Users;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Features.Identity.Users.Queries.GetAllGuestsPaging
{
    public class GetAllGuestsPagingQuery : PagingDto, IRequest<ErrorOr<IResponseWrapper<PagedResult<UserDto>>>>
    {
    }
}
