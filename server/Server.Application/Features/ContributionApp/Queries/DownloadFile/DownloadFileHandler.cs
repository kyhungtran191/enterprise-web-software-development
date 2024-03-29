using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.ContributionApp.Queries.DownloadFile
{
    public class DownloadFileHandler : IRequestHandler<DownloadFileQuery, ErrorOr<(Stream fileStream, string contentType, string fileName)>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediaService _mediaService;

        public DownloadFileHandler(IUnitOfWork unitOfWork, IMediaService mediaService)
        {
            _unitOfWork = unitOfWork;
            _mediaService = mediaService;
        }
        public async Task<ErrorOr<(Stream fileStream, string contentType, string fileName)>> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            var filePaths = await _unitOfWork.FileRepository.GetPathByContribution(request.ContributionId);
            if (filePaths.Count == 0 || filePaths.Contains(""))
            {
                return Errors.Contribution.NoFilesFound;
            }

           return await _mediaService.DownloadFiles(filePaths);
        }
    }
}
