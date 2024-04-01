using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDataProtector _dataProtector;
        public ResetPasswordCommandHandler(UserManager<AppUser> userManager, IDataProtectionProvider dataProtectionProvider)
        {
            _userManager = userManager;
            _dataProtector = dataProtectionProvider.CreateProtector("DataProtectorTokenProvider");

        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
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
    }
}
