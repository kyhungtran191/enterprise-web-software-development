using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.DownSingleFile
{
    public class DownSingleFileHandler : IRequestHandler<DownSingleFileQuery, ErrorOr<IResponseWrapper<string>>>
    {
        private readonly IMediaService _mediaService;
        public DownSingleFileHandler(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }
        public async Task<ErrorOr<IResponseWrapper<string>>> Handle(DownSingleFileQuery request, CancellationToken cancellationToken)
        {
            var result = _mediaService.GenerateDownloadUrl(request.PublicIds);
            return new ResponseWrapper<string>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Link download below" },
                ResponseData = result
            };
        }
    }
}
