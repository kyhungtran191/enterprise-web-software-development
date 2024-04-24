using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.ContributionApp.Queries.GetActivityLog;
using Server.Application.Features.ContributionApp.Queries.GetNotCommentContribution;
using Server.Contracts.Contributions;
using Server.Domain.Common.Constants;

namespace Server.Api.Controllers.ManagerApi
{
    public class ContributionsController : ManagerApiController
    {
        private readonly IMapper _mapper;
        public ContributionsController(ISender mediatorSender, IMapper mapper) : base(mediatorSender)
        {
            _mapper = mapper;
        }
        [HttpGet("activity-logs/{ContributionId}")]
        [Authorize(Permissions.ActivityLogs.View)]
        public async Task<IActionResult> GetActivityLogs([FromRoute] GetActivityLogRequest request)
        {
            var query = _mapper.Map<GetActivityLogQuery>(request);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }
        [HttpGet("not-comment-contribution")]
        [Authorize(Permissions.NotCommentContribution.View)]
        public async Task<IActionResult> GetNotCommentContribution([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetNotCommentContributionQuery();
            query.PageIndex = pageIndex;
            query.PageSize = pageSize;
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }
    }
}
