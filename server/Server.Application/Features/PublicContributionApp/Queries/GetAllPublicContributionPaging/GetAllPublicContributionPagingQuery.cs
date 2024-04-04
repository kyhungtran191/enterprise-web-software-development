using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Features.PublicContributionApp.Queries.GetAllPublicContributionPaging
{
    public class GetAllPublicContributionPagingQuery : PagingDto, IRequest<ErrorOr<IResponseWrapper<PagedResult<PublicContributionInListDto>>>>
    {
        public string? Year { get; set; }
        public string? FacultyName { get; set; }
        public string? Status { get; set; }
        public bool? GuestAllowed { get; set; }
    }
}
