using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.DownAllFile
{
    public class DownloadAllFileQuery : IRequest<ErrorOr<IResponseWrapper<string>>>
    {
        public Guid ContributionId { get; set; }
    }
}
