using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos;
using Server.Application.Common.Dtos.AcademicYears;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Features.AcademicYearApp.Queries.GetAllAcademicYearPaging
{
    public class GetAllAcademicYearsPagingQuery : PagingDto, IRequest<ErrorOr<IResponseWrapper<PagedResult<AcademicYearDto>>>>
    {
    }
}
