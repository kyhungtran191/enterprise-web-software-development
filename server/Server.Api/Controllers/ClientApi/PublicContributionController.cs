using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Common.Extensions;
using Server.Application.Features.PublicContributionApp.Commands.LikeContribution;
using Server.Application.Features.PublicContributionApp.Commands.ViewContribution;
using Server.Application.Features.PublicContributionApp.Queries.GetAllPublicContributionPaging;
using Server.Application.Features.PublicContributionApp.Queries.GetTop4Contributions;
using Server.Contracts.PublicContributions;
using Server.Contracts.PublicContributions.Like;
using Server.Contracts.PublicContributions.View;

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
        var query = new GetTop4ContributionQuery();
        var result = await _mediatorSender.Send(query);
        return result.Match(success => Ok(success), errors => Problem(errors));
    }

    [HttpPost]
    [Route("toggle-like/{ContributionId}")]
    public async Task<IActionResult> LikeContribution([FromRoute] LikeContributionRequest request)
    {
        var command = _mapper.Map<LikeContributionCommand>(request);
        command.UserId = User.GetUserId();
        var result = await _mediatorSender.Send(command);
        return result.Match(success => Ok(success), errors => Problem(errors));
    }

    [HttpPost]
    [Route("view/{ContributionId}")]
    public async Task<IActionResult> ViewContribution([FromRoute] ViewContributionRequest request)
    {
        var command = _mapper.Map<ViewContributionCommand>(request);
        var result = await _mediatorSender.Send(command);
        return result.Match(success => Ok(success), errors => Problem(errors));
    }
}