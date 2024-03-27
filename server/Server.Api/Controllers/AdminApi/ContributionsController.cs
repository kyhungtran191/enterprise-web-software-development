using System.Diagnostics;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.ContributionApp.Commands.DeleteContribution;
using Server.Application.Features.ContributionApp.Queries.GetAllContributionsPaging;
using Server.Application.Features.ContributionApp.Queries.GetContributionByTitle;
using Server.Contracts.Contributions;

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
        public async Task<IActionResult> GetContributionBySlug([FromRoute] GetContributionBySlugRequest getContributionBySlugRequest)
        {
            var query = _mapper.Map<GetContributionBySlugQuery>(getContributionBySlugRequest);
            Debug.WriteLine(query.Slug);
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
