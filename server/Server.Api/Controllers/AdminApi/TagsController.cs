using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.TagApp.Commands.CreateTag;
using Server.Application.Features.TagApp.Commands.DeleteTag;
using Server.Application.Features.TagApp.Commands.UpdateTag;
using Server.Application.Features.TagApp.Queries.GetAllTagsPaging;
using Server.Application.Features.TagApp.Queries.GetTagById;
using Server.Contracts.Tags;
using Server.Domain.Common.Constants;

namespace Server.Api.Controllers.AdminApi
{

    public class TagsController : AdminApiController
    {
        private readonly IMapper _mapper;

        public TagsController(IMapper mapper, IMediator sender) : base(sender)
        {
            _mapper = mapper;
        }
        [HttpGet]
        [Route("{TagId}")]
        [Authorize(Permissions.Tags.View)]
        public async Task<IActionResult> GetTagById([FromRoute] GetTagByIdRequest getTagByIdRequest)
        {
            var query = _mapper.Map<GetTagByIdQuery>(getTagByIdRequest);
            var result = await _mediatorSender.Send(query);
            return result.Match(success => Ok(success), errors => Problem(errors));

        }

        [HttpGet]
        [Route("paging")]
        [Authorize(Permissions.Tags.View)]
        public async Task<IActionResult> GetAllTagsByPaging([FromQuery] GetAllTagsPagingRequest getAllTagsPagingRequest)
        {
            var query = _mapper.Map<GetAllTagsPagingQuery>(getAllTagsPagingRequest);
            var result = await _mediatorSender.Send(query);
            return result.Match(success => Ok(success), errors => Problem(errors));
        }

        [HttpPost]
        [Authorize(Permissions.Tags.Create)]
        public async Task<IActionResult> CreateNewTag(CreateTagRequest createTagRequest)
        {
            var command = _mapper.Map<CreateTagCommand>(createTagRequest);
            var result = await _mediatorSender.Send(command);
            return result.Match(
                success => Ok(success),
                errors => Problem(errors)
            );
        }

        [HttpPut]
        [Authorize(Permissions.Tags.Edit)]
        public async Task<IActionResult> UpdateTag(UpdateTagRequest updateTagRequest)
        {
            var command = _mapper.Map<UpdateTagCommand>(updateTagRequest);
            var result = await _mediatorSender.Send(command);
            return result.Match(
                success => Ok(success),
                errors => Problem(errors)
            );
        }

        [HttpDelete]
        [Authorize(Permissions.Tags.Delete)]
        public async Task<IActionResult> DeleteTagById(DeleteTagRequest deleteTagRequest)
        {
            var command = _mapper.Map<DeleteTagCommand>(deleteTagRequest);
            var result = await _mediatorSender.Send(command);
            return result.Match(
                success => Ok(success),
                errors => Problem(errors)
            );
        }

    }
}
