using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.ContributionApp.Queries.DownloadFile
{
    public class DownloadFileHandler : IRequestHandler<DownloadFileQuery, ErrorOr<IResponseWrapper<string>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediaService _mediaService;

        public DownloadFileHandler(IUnitOfWork unitOfWork, IMediaService mediaService)
        {
            _unitOfWork = unitOfWork;
            _mediaService = mediaService;
        }
        public async Task<ErrorOr<IResponseWrapper<string>>> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            var filePaths = await _unitOfWork.FileRepository.GetPathByContribution(request.ContributionId);
            if (filePaths.Count == 0 || filePaths.Contains(""))
            {
                return Errors.Contribution.NoFilesFound;
            }
            var result = _mediaService.GenerateDownloadUrl(filePaths);

            return new ResponseWrapper<string>
            {
                IsSuccessfull = true,
                Messages = new List<string>{ $"Link download below"},
                ResponseData = result
            };
        }
    }
}
