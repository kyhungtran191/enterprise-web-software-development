using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.Identity.Users.Commands.ForgotPassword;
using Server.Application.Features.Identity.Users.Commands.ResetPassword;
using Server.Contracts.Identity.Users;

namespace Server.Api.Controllers.ClientApi
{
    public class UsersController : ClientApiController
    {
        private readonly IMapper _mapper;
        public UsersController(ISender mediatorSender,IMapper mapper) : base(mediatorSender)
        {
            _mapper = mapper;
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var command = _mapper.Map<ForgotPasswordCommand>(request);
            var result = await _mediatorSender.Send(command);
            return result.Match(success => Ok(success), errors => Problem(errors));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var command = _mapper.Map<ResetPasswordCommand>(request);
            var result = await _mediatorSender.Send(command);
            return result.Match(success => Ok(success), errors => Problem(errors));
        }
    }
}
