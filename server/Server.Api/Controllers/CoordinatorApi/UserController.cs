using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Common.Extensions;
using Server.Application.Features.Identity.Users.Commands.CreateGuest;
using Server.Application.Features.Identity.Users.Queries.GetAllGuestsPaging;
using Server.Application.Features.Identity.Users.Queries.GetAllUsersPaging;
using Server.Contracts.Identity.Users;

namespace Server.Api.Controllers.CoordinatorApi
{
    public class UsersController  : CoordinatorApiController
    {
        private readonly IMapper _mapper;
        public UsersController(ISender _sender,IMapper mapper) : base(_sender)
        {
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create-guest")]
        public async Task<IActionResult> CreateGuest([FromForm] CreateGuestRequest request)
        {
            var command = _mapper.Map<CreateGuestCommand>(request);
            command.FacultyId = User.GetFacultyId();
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpGet]
        [Route("view-guest")]
        public async Task<IActionResult> ViewAllGuest([FromQuery] GetAllUserPagingRequest getAllUserPagingRequest)
        {
            var query = _mapper.Map<GetAllGuestsPagingQuery>(getAllUserPagingRequest);

            var result = await _mediatorSender.Send(query);

            return result.Match(
                successResult => Ok(successResult),
                errors => Problem(errors)
            );
        }
    }
}
