using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos;
using Server.Application.Common.Dtos.Faculties;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Features.FacultyApp.Queries.GetAllFacutiesPaging;

public class GetAllFacultiesPagingQuery : PagingDto, IRequest<ErrorOr<IResponseWrapper<PagedResult<FacultyDto>>>>
{    

}