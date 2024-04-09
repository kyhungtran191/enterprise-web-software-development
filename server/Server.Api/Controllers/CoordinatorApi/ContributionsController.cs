using System.Runtime.CompilerServices;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Common.Filters;
using Server.Application.Common.Extensions;
using Server.Application.Features.CommentApp.Commands;
using Server.Application.Features.ContributionApp.Commands.ApproveContributions;
using Server.Application.Features.ContributionApp.Commands.RejectContribution;
using Server.Application.Features.ContributionApp.Commands.UpdateContribution;
using Server.Application.Features.ContributionApp.Queries.GetActivityLog;
using Server.Application.Features.ContributionApp.Queries.GetAllContributionsPaging;
using Server.Application.Features.ContributionApp.Queries.GetContributionByTitle;
using Server.Application.Features.ContributionApp.Queries.GetCoordinatorContribution;
using Server.Application.Features.ContributionApp.Queries.GetRejectReason;
using Server.Application.Features.PublicContributionApp.Commands.AllowGuest;
using Server.Contracts.Comment;
using Server.Contracts.Contributions;
using Server.Contracts.PublicContributions;
using Server.Domain.Common.Constants;

namespace Server.Api.Controllers.CoordinatorApi
{
    public class ContributionsController : CoordinatorApiController
    {
        private readonly IMapper _mapper;
        public ContributionsController(ISender _mediator, IMapper mapper) : base(_mediator)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [Route("paging")]
        [Authorize(Permissions.Contributions.View)]
        public async Task<IActionResult> GetFacultyContribution([FromQuery] GetFacultyContributionRequest request)
        {
            var query = _mapper.Map<GetAllContributionsPagingQuery>(request);
            query.FacultyName = User.GetFacultyName();
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpPost("approve")]
        [Authorize(Permissions.Contributions.Approve)]
        public async Task<IActionResult> ApproveContribution(ApproveContributionsRequest request)
        {
            var command = _mapper.Map<ApproveContributionsCommand>(request);
            command.UserId = User.GetUserId();
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpPost("reject")]
        [Authorize(Permissions.Contributions.Approve)]
        public async Task<IActionResult> RejectContribution(RejectContributionRequest request)
        {
            var command = _mapper.Map<RejectContributionCommand>(request);
            command.UserId = User.GetUserId();
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpGet("reject-reason/{Id}")]
        [Authorize(Permissions.Contributions.Approve)]
        public async Task<IActionResult> GetRejectReason([FromRoute] GetRejectReasonRequest request)
        {
            var query = _mapper.Map<GetRejectReasonQuery>(request);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }
        [HttpGet("activity-logs/{ContributionId}")]
        [Authorize(Permissions.Contributions.Approve)]
        public async Task<IActionResult> GetActivityLogs([FromRoute] GetActivityLogRequest request)
        {
            var query = _mapper.Map<GetActivityLogQuery>(request);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }
        [HttpGet]
        [Route("preview-contribution/{Slug}")]
        [Authorize(Permissions.Contributions.View)]
        public async Task<IActionResult> GetContributionBySlug([FromRoute] GetContributionBySlugRequest getContributionBySlugRequest)
        {
            var query = _mapper.Map<GetCoordinatorContributionQuery>(getContributionBySlugRequest);
            query.FacultyName = User.GetFacultyName();
            query.UserId = User.GetUserId();
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }

        [HttpPost]
        [Route("allow-guest")]
        public async Task<IActionResult> AllowGuest(AllowGuestRequest request)
        {
            var command = _mapper.Map<AllowGuestCommand>(request);
            command.FacultyId = User.GetFacultyId();
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpPost]
        [Route("comment/{ContributionId}")]
        public async Task<IActionResult> Comment([FromRoute]Guid ContributionId, CreateCommentRequest request )
        {
            var command = _mapper.Map<CreateCommentCommand>(request);
            command.ContributionId = ContributionId;
            command.UserId = User.GetUserId();
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }
    }
}
