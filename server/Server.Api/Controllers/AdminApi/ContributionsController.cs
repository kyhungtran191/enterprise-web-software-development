﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Common.Extensions;
using Server.Application.Features.ContributionApp.Commands.ApproveContributions;
using Server.Application.Features.ContributionApp.Commands.DeleteContribution;
using Server.Application.Features.ContributionApp.Queries.GetAllContributionsPaging;
using Server.Application.Features.ContributionApp.Queries.GetContributionByTitle;
using Server.Contracts.Contributions;
using System.Diagnostics;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Features.ContributionApp.Commands.RejectContribution;
using Server.Application.Features.ContributionApp.Queries.GetActivityLog;
using Server.Application.Features.ContributionApp.Queries.GetRejectReason;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveContribution(ApproveContributionsRequest request)
        {
            var command = _mapper.Map<ApproveContributionsCommand>(request);
            command.UserId = User.GetUserId();
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectContribution( RejectContributionRequest request)
        {
            var command = _mapper.Map<RejectContributionCommand>(request);
            command.UserId = User.GetUserId();
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpGet("reject-reason/{Id}")]
        public async Task<IActionResult> GetRejectReason([FromRoute] GetRejectReasonRequest request)
        {
            var query = _mapper.Map<GetRejectReasonQuery>(request);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }
        [HttpGet("activity-logs/{ContributionId}")]
        public async Task<IActionResult> GetActivityLogs([FromRoute] GetActivityLogRequest request)
        {
            var query = _mapper.Map<GetActivityLogQuery>(request);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }

    }
}