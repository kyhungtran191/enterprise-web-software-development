using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.Application.Features.PublicContributionApp.Queries.DownAllFile
{
    public class DownloadAllFileHandler : IRequestHandler<DownloadAllFileQuery, ErrorOr<IResponseWrapper<string>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediaService _mediaService;

        public DownloadAllFileHandler(IUnitOfWork unitOfWork, IMediaService mediaService)
        {
            _unitOfWork = unitOfWork;
            _mediaService = mediaService;
        }
        public async Task<ErrorOr<IResponseWrapper<string>>> Handle(DownloadAllFileQuery request, CancellationToken cancellationToken)
        {
            var itemFromDb = await _unitOfWork.PublicContributionRepository.GetByIdAsync(request.ContributionId);
            if (itemFromDb is null)
            {
                return Errors.Contribution.NotFoundPublic;
            }
            var filePaths = await _unitOfWork.FileRepository.GetPathByContribution(request.ContributionId);
            if (filePaths.Count == 0 || filePaths.Contains(""))
            {
                return Errors.Contribution.NoFilesFound;
            }
            var result = _mediaService.GenerateDownloadUrl(filePaths);

            return new ResponseWrapper<string>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Link download below" },
                ResponseData = result
            };
        }
    }
}
