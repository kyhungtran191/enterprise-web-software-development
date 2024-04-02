using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.ContributionApp.Commands.DeleteContribution;
using Server.Application.Features.ContributionApp.Queries.DownloadFile;
using Server.Application.Features.ContributionApp.Queries.GetAllContributionsPaging;
using Server.Application.Features.ContributionApp.Queries.GetContributionByTitle;
using Server.Contracts.Contributions;
using Server.Domain.Common.Constants;

namespace Server.Api.Controllers.AdminApi
{

    public class ContributionsController : AdminApiController
    {
        private readonly IMapper _mapper;
        public ContributionsController(ISender _mediator,IMapper mapper) : base(_mediator)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{Slug}")]
        [Authorize(Permissions.Contributions.View)]
        public async Task<IActionResult> GetContributionBySlug([FromRoute] GetContributionBySlugRequest getContributionBySlugRequest)
        {
            var query = _mapper.Map<GetContributionBySlugQuery>(getContributionBySlugRequest);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }

        [HttpGet]
        [Route("paging")]
        [Authorize(Permissions.Contributions.View)]
        public async Task<IActionResult> GetAllContributionsPaging([FromQuery] GetAllContributionPagingRequest getAllContributionPagingRequest)
        {
            var query = _mapper.Map<GetAllContributionsPagingQuery>(getAllContributionPagingRequest);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

      
        [HttpDelete]
        [Authorize(Permissions.Contributions.Delete)]
        public async Task<IActionResult> DeleteContribution(DeleteContributionRequest deleteContributionRequest)
        {
            var command = _mapper.Map<DeleteContributionCommand>(deleteContributionRequest);
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }

     
        [HttpGet("download-file/{ContributionId}")]
        [Authorize(Permissions.Contributions.Approve)]
        public async Task<IActionResult> DownloadFile([FromRoute] DownloadFileRequest request)
        {
            var query = _mapper.Map<DownloadFileQuery>(request);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));


        }


    }
}
