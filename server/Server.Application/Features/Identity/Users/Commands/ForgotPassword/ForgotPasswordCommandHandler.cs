using System.Security.Policy;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Contracts.Common;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Users.Commands.ForgotPassword
{
  
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ErrorOr<IResponseWrapper<string>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ForgotPasswordCommandHandler(UserManager<AppUser> userManager, IEmailService emailService,IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }
      public async Task<ErrorOr<IResponseWrapper<string>>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
      {
          var user = await _userManager.FindByEmailAsync(request.Email);
          if (user is null)
          {
              return Errors.User.CannotFound;
          }

          var token = await _userManager.GeneratePasswordResetTokenAsync(user);
          var resetPasswordBaseUrl = _configuration["ApplicationSettings:ResetPasswordBaseUrl"];
          var resetPasswordUrl = $"{resetPasswordBaseUrl}?token={Uri.EscapeDataString(token)}";
          var emailBody = $"Please reset your password by clicking the following link: {resetPasswordUrl}";
            _emailService.SendEmail(new MailRequest
          {
              ToEmail = user?.Email,
              Body = emailBody,
              Subject = "RESET PASSWORD"
          });
           return new ResponseWrapper<string>
           {
               IsSuccessfull = true,
               Messages = new List<string> { $"Sent email to user {user.UserName} success. Please check email" },
               ResponseData = token,
           };

      }
    }
}
