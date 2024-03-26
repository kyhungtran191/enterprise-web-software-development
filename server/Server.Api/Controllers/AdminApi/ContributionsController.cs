using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.ContributionApp.Commands.CreateContribution;
using Server.Application.Features.ContributionApp.Queries.GetContributionByTitle;
using Server.Contracts.Contributions;
using System.Security.Claims;
using Server.Application.Common.Extensions;
using Server.Application.Features.ContributionApp.Commands.DeleteContribution;
using Server.Application.Features.ContributionApp.Commands.UpdateContribution;
using Server.Application.Features.ContributionApp.Queries.GetAllContributionsPaging;
using ErrorOr;

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
        [Route("{Title}")]
        public async Task<IActionResult> GetContributionByName([FromRoute] GetContributionByTitleRequest getContributionByTitleRequest)
        {
            var query = _mapper.Map<GetContributionByTitleQuery>(getContributionByTitleRequest);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }

        [HttpGet]
        [Route("paging")]
        public async Task<IActionResult> GetAllContributionsPaging([FromQuery] GetAllContributionPagingRequest getAllContributionPagingRequest)
        {
            var query = _mapper.Map<GetAllContributionsPagingQuery>(getAllContributionPagingRequest);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

      
        [HttpDelete]
        public async Task<IActionResult> DeleteContribution(DeleteContributionRequest deleteContributionRequest)
        {
            var command = _mapper.Map<DeleteContributionCommand>(deleteContributionRequest);
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }

        //[HttpGet("tags/{contributionId}")]
        //public async Task<IActionResult> GetTagsOfContribution([FromRoute]  GetTagsContributionRequest getTagsContributionRequest)
        //{

        //}
    }
}
