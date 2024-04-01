using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Queries.DownloadFile
{
    public class DownloadFileQuery : IRequest<ErrorOr<IResponseWrapper<string>>>
    {
        public Guid ContributionId { get; set; }
    }
}
