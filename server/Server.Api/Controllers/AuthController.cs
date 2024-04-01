using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.Authentication;
using Server.Contracts.Authentication;
using Server.Contracts.Authentication.Requests;

namespace Server.Api.Controllers;

[AllowAnonymous]
[Route("[controller]")]
public class AuthController : ApiController
{
    private readonly IMapper _mapper;
    public AuthController(ISender mediatorSender, IMapper mapper) : base(mediatorSender)
    {
        _mapper = mapper;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var query = _mapper.Map<LoginQuery>(loginRequest);

        var loginResult = await _mediatorSender.Send(query);

        return loginResult.Match(
            loginResultSuccess => Ok(_mapper.Map<AuthenticationResponse>(loginResultSuccess)),
            errors => Problem(errors)
        );
    }
}