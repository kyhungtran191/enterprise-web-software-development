using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Features.Identity.Users.Commands.CreateUser;
using Server.Application.Features.Identity.Users.Commands.DeleteUserById;
using Server.Application.Features.Identity.Users.Commands.UpdateUser;
using Server.Application.Features.Identity.Users.Queries.GetAllUsersPaging;
using Server.Application.Features.Identity.Users.Queries.GetUserById;
using Server.Contracts.Common;
using Server.Contracts.Identity.Users;
using Server.Domain.Common.Constants;

namespace Server.Api.Controllers.AdminApi;

public class UsersController : AdminApiController
{
    private readonly IMapper _mapper;
    public UsersController(ISender mediatorSender, IMapper mapper) : base(mediatorSender)
    {
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize(Permissions.Users.Create)]
    public async Task<IActionResult> CreateUser([FromForm] CreateUserRequest createUserRequest)
    {
        var command = _mapper.Map<CreateUserCommand>(createUserRequest);

        var createUserResult = await _mediatorSender.Send(command);

        return createUserResult.Match(
            successResult => Ok(successResult),
            errors => Problem(errors)
        );
    }

    [HttpPut]
    [Authorize(Permissions.Users.Edit)]
    public async Task<IActionResult> UpdateUser(UpdateUserRequest updateUserRequest)
    {
        var command = _mapper.Map<UpdateUserCommand>(updateUserRequest);

        var updateUserResult = await _mediatorSender.Send(command);

        return updateUserResult.Match(
            successResult => Ok(successResult),
            errors => Problem(errors)
        );
    }

    [HttpGet]
    [Authorize(Permissions.Users.View)]
    public async Task<IActionResult> GetAllUsersPaging([FromQuery] GetAllUserPagingRequest getAllUserPagingRequest)
    {
        var query = _mapper.Map<GetAllUserPagingQuery>(getAllUserPagingRequest);

        var result = await _mediatorSender.Send(query);

        return result.Match(
            successResult => Ok(successResult),
            errors => Problem(errors)
        );
    }

    [HttpGet]
    [Route("{id}")]
    [Authorize(Permissions.Users.View)]
    public async Task<IActionResult> GetUserById([FromRoute] GetUserByIdRequest getUserByIdRequest)
    {
        var query = _mapper.Map<GetUserByIdQuery>(getUserByIdRequest);

        var result = await _mediatorSender.Send(query);

        return result.Match(
            successResult => Ok(successResult),
            errors => Problem(errors)
        );
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Permissions.Users.Delete)]
    public async Task<IActionResult> DeleteUserById([FromRoute] DeleteUserByIdRequest deleteUserByIdRequest)
    {
        var query = _mapper.Map<DeleteUserByIdCommand>(deleteUserByIdRequest);

        var result = await _mediatorSender.Send(query);

        return result.Match(
            successResult => Ok(successResult),
            errors => Problem(errors)
        );
    }
}