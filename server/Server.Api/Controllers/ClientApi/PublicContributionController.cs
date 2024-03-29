using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.PublicContributionApp.Queries.GetAllPublicContributionPaging;
using Server.Contracts.PublicContributions;

namespace Server.Api.Controllers.ClientApi;

public class PublicContributionController : ClientApiController
{
    private readonly IMapper _mapper;
    public PublicContributionController(ISender mediatorSender, IMapper mapper) : base(mediatorSender)
    {
        _mapper = mapper;
    }

    [HttpGet]
    [Route("paging")]
    public async Task<IActionResult> GetAllPublicContribution([FromQuery] GetAllPublicContributionPagingRequest request)
    {
        var query = _mapper.Map<GetAllPublicContributionPagingQuery>(request);
        var result = await _mediatorSender.Send(query);
        return result.Match(success => Ok(success), errors => Problem(errors));

    }

    [HttpGet]
    [Route("top-4")]
    public async Task<IActionResult> GetTop4Contributions()
    {
        var query = new GetAllPublicContributionPagingQuery();
        var result = await _mediatorSender.Send(query);
        return result.Match(success => Ok(success), errors => Problem(errors));
    }

}