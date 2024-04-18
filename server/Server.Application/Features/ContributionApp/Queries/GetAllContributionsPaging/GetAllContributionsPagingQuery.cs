using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Features.ContributionApp.Queries.GetAllContributionsPaging
{
    public class GetAllContributionsPagingQuery : PagingDto, IRequest<ErrorOr<IResponseWrapper<PagedResult<ContributionInListDto>>>>
    {
        public string? Year  {get; set; }
        public string? FacultyName { get; set; }
        public Guid? UserId { get; set; }
        public string? Status { get; set; }
        public bool? GuestAllowed { get; set; }
    }
}
