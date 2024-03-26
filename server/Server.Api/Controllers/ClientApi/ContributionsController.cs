using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Common.Filters;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Features.ContributionApp.Commands.CreateContribution;
using Server.Application.Features.ContributionApp.Commands.UpdateContribution;
using Server.Contracts.Contributions;
using Server.Domain.Common.Constants;

namespace Server.Api.Controllers.ClientApi
{ 
    [Authorize]
   public class ContributionsController : ClientApiController
    {
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        public ContributionsController(ISender mediatorSender,IMapper mapper,IMediaService mediaService) : base(mediatorSender)
        {
            _mapper = mapper;
            _mediaService = mediaService;
        }
        [HttpPost]
        [FileValidationFilter(5*1024*1024)]
        public async Task<IActionResult> CreateContribution([FromForm] CreateContributionRequest createContributionRequest)
        {
            var command = _mapper.Map<CreateContributionCommand>(createContributionRequest);
            command.UserId = User.GetUserId();
          

            var thumbnailFileInfo = await _mediaService.UploadFiles(createContributionRequest.Thumbnail, FileType.Thumbnail);
            var fileInfo = await _mediaService.UploadFiles(createContributionRequest.File, FileType.File);
            command.ThumbnailInfo = thumbnailFileInfo;
            command.FileInfo = fileInfo;
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }

        [HttpPut]
        public async Task<IActionResult> UpdateContribution(UpdateContributionRequest updateContributionRequest)
        {
            var command = _mapper.Map<UpdateContributionCommand>(updateContributionRequest);
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

    }
}
