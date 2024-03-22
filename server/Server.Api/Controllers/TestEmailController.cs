using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Common.Interfaces.Services;
using Server.Contracts.Common;

namespace Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestEmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public TestEmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost("sendmail")]
        public async Task<IActionResult> SendMail()
        {
            MailRequest mailRequest = new MailRequest
            {
                ToEmail = "nguahoang2003@gmail.com",
                Subject = "Test",
                Body = "Test"
            };
            _emailService.SendEmail(mailRequest);
            return Ok();
        }
    }
}
