using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.Identity.Tokens.Commands.RefreshToken;
using Server.Contracts.Identity.Tokens;

namespace Server.Api.Controllers;


[Route("api/[controller]")]
public class TokensController : ApiController
{
    private readonly IMapper _mapper;
    public TokensController(ISender mediatorSender, IMapper mapper)
        : base(mediatorSender)
    {
        _mapper = mapper;
    }

    [HttpPost]
    [Route("Refresh")]
    public async Task<IActionResult> Refresh(TokenRequest tokenRequest)
    {
        var command = _mapper.Map<RefreshTokenCommand>(tokenRequest);

        var result = await _mediatorSender.Send(command);

        return result.Match(
            successResult => Ok(successResult),
            erros => Problem(erros)
        );
    }

}