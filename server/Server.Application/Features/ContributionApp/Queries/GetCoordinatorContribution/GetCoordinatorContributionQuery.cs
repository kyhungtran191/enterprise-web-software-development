using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Queries.GetCoordinatorContribution
{
    public class GetCoordinatorContributionQuery : IRequest<ErrorOr<IResponseWrapper<ContributionWithCommentDto>>>
    {
        public string Slug { get; set; }
        public string FacultyName { get; set; }
        public Guid UserId { get; set; }
    }
}
