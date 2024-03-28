using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetPasswordCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Errors.User.CannotFound;
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!resetPassResult.Succeeded)
            {
                return Errors.User.FailResetPassword;
            }

            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Reset password success" }
            };
        }
    }
}
