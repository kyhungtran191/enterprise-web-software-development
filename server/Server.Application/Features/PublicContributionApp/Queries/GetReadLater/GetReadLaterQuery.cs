using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.GetReadLater
{
    public class GetReadLaterQuery : IRequest<ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>>
    {
        public Guid UserId { get; set; }
    }
}
