using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Common.Extensions;
using Server.Application.Features.ContributionApp.Queries.GetAllContributionsPaging;
using Server.Application.Features.ContributionApp.Queries.GetContributionByTitle;
using Server.Application.Features.ContributionApp.Queries.GetUserContribution;
using Server.Application.Features.Identity.Users.Commands.ForgotPassword;
using Server.Application.Features.Identity.Users.Commands.ResetPassword;
using Server.Application.Features.Identity.Users.Commands.UpdateProfile;
using Server.Application.Features.Identity.Users.Queries.GetProfile;
using Server.Application.Features.Identity.Users.Queries.ValidateForgotToken;
using Server.Application.Features.PublicContributionApp.Commands.CreateReadLater;
using Server.Application.Features.PublicContributionApp.Queries.GetFavorite;
using Server.Application.Features.PublicContributionApp.Queries.GetLikedContribution;
using Server.Application.Features.PublicContributionApp.Queries.GetReadLater;
using Server.Contracts.Contributions;
using Server.Contracts.Identity.Users;
using Server.Contracts.PublicContributions.ReadLater;
using Server.Domain.Common.Constants;

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

        [HttpPost("validate-forgot-token")]
        public async Task<IActionResult> ValidateToken(ValidateForgotTokenRequest request)
        {
            var query = _mapper.Map<ValidateForgotTokenQuery>(request);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }
        [HttpGet("recent-contribution")]
        [Authorize]
        [Authorize(Permissions.StudentContribution.View)]
        public async Task<IActionResult> GetContributionOfUser([FromQuery] GetAllContributionPagingRequest request)
        {
            var query = _mapper.Map<GetAllContributionsPagingQuery>(request);
            query.UserId = User.GetUserId();
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpGet("my-profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var query = new GetProfileQuery { UserId = User.GetUserId() };
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpPost("edit-profile")]
        [Authorize]
        public async Task<IActionResult> EditProfile([FromForm] UpdateProfileRequest request)
        {
            var command = _mapper.Map<UpdateProfileCommand>(request);
            command.UserId = User.GetUserId();
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpGet("read-later")]
        [Authorize(Permissions.ReadLaterContribution.View)]
        public async Task<IActionResult> GetReadLater()
        {
            var query = new GetReadLaterQuery();
            query.UserId = User.GetUserId();
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }
        [HttpGet("my-favorite")]
        [Authorize(Permissions.FavoriteContribution.View)]
        public async Task<IActionResult> GetLikedContribution()
        {
            var query = new GetLikedContributionQuery();
            query.UserId = User.GetUserId();
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpGet]
        [Route("contribution/{Slug}")]
        [Authorize(Permissions.Contributions.Edit)]
        public async Task<IActionResult> GetContributionBySlug([FromRoute] GetContributionBySlugRequest getContributionBySlugRequest)
        {
            var query = _mapper.Map<GetUserContributionQuery>(getContributionBySlugRequest);
            query.UserId = User.GetUserId();
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }   
        //[HttpGet("my-favorite")]
        //[Authorize]
        //public async Task<IActionResult> GetFavorite()
        //{
        //    var query = new GetFavoriteQuery();
        //    query.UserId = User.GetUserId();
        //    var result = await _mediatorSender.Send(query);
        //    return result.Match(result => Ok(result), errors => Problem(errors));
        //}
    }
}
