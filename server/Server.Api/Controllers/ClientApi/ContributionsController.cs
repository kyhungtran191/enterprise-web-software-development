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
using Server.Application.Features.ContributionApp.Queries.GetAllContributionsPaging;
using Server.Contracts.Contributions;
using Server.Domain.Common.Constants;

namespace Server.Api.Controllers.ClientApi
{ 
    [Authorize]
   public class ContributionsController : ClientApiController
    {
        private readonly IMapper _mapper;

        public ContributionsController(ISender mediatorSender, IMapper mapper) : base(
            mediatorSender)
        {
            _mapper = mapper;
        }

        [HttpPost]
        [FileValidationFilter(5*1024*1024)]
        [Authorize(Permissions.Contributions.Create)]
        public async Task<IActionResult> CreateContribution([FromForm] CreateContributionRequest createContributionRequest)
        {
            var command = _mapper.Map<CreateContributionCommand>(createContributionRequest);
            command.UserId = User.GetUserId();
            command.FacultyId = User.GetFacultyId();
            command.Slug = createContributionRequest.Title.Slugify();
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }

        [HttpPut]
        [FileValidationFilter(5 * 1024 * 1024)]
        [Authorize(Permissions.Contributions.Edit)]
        public async Task<IActionResult> UpdateContribution([FromForm] UpdateContributionRequest updateContributionRequest)
        {
            var command = _mapper.Map<UpdateContributionCommand>(updateContributionRequest);
            command.UserId = User.GetUserId();
            command.FacultyId = User.GetFacultyId();
            command.Slug = updateContributionRequest.Title.Slugify();
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        


    }
}
