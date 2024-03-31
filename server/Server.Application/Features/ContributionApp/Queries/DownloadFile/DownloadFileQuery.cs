using ErrorOr;
using MediatR;

namespace Server.Application.Features.ContributionApp.Queries.DownloadFile
{
    public class DownloadFileQuery : IRequest<ErrorOr<(Stream fileStream, string contentType, string fileName)>>
    {
        public Guid ContributionId { get; set; }
    }
}
