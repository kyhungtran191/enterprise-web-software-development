using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Users.Queries.ValidateForgotToken
{
    public class ValidateForgotTokenHandler : IRequestHandler<ValidateForgotTokenQuery, ErrorOr<IResponseWrapper<bool>>>
    {
        private readonly UserManager<AppUser> _userManager;
        public ValidateForgotTokenHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ErrorOr<IResponseWrapper<bool>>> Handle(ValidateForgotTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return Errors.User.CannotFound;
            }

            return new ResponseWrapper<bool>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Validate token success" },
                ResponseData = await _userManager.VerifyUserTokenAsync(user,
                    _userManager.Options.Tokens.PasswordResetTokenProvider,
                    "ResetPassword", request.Token)
            };
        }
            
    }
}
