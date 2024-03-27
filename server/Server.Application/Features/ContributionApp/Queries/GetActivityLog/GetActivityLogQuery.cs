using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Queries.GetActivityLog
{
    public class GetActivityLogQuery : IRequest<ErrorOr<IResponseWrapper<List<ActivityLogDto>>>>
    {
        public Guid ContributionId { get; set; }
    }
}
