using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Users.Queries.ValidateForgotToken
{
    public class ValidateForgotTokenHandler : IRequestHandler<ValidateForgotTokenQuery, ErrorOr<IResponseWrapper<bool>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDataProtector _dataProtector;
        public ValidateForgotTokenHandler(UserManager<AppUser> userManager, IDataProtectionProvider dataProtectionProvider)
        {
            _userManager = userManager;
            _dataProtector = dataProtectionProvider.CreateProtector("DataProtectorTokenProvider");
        }
        public async Task<ErrorOr<IResponseWrapper<bool>>> Handle(ValidateForgotTokenQuery request, CancellationToken cancellationToken)
        {
          
            var resetTokenArray = Convert.FromBase64String(request.Token);

            var unprotectedResetTokenArray = _dataProtector.Unprotect(resetTokenArray);

            using (var ms = new MemoryStream(unprotectedResetTokenArray))
            {
                using (var reader = new BinaryReader(ms))
                {
                    reader.ReadInt64();
                    var userId = reader.ReadString();
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user == null)
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
            
    }
}
