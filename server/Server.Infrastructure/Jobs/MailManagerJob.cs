using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using Server.Application.Common.Interfaces.Services;
using Server.Contracts.Common;
using Server.Domain.Common.Constants;
using Server.Domain.Entity.Identity;


namespace Server.Infrastructure.Jobs
{
    [DisallowConcurrentExecution]
    public class MailManagerJob : IJob
    {
        private readonly AppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<MailManagerJob> _logger;
        private readonly IConfiguration _configuration;

        public MailManagerJob(AppDbContext appDbContext, IEmailService emailService,UserManager<AppUser> userManager, IConfiguration configuration, ILogger<MailManagerJob> logger)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
          
        }
        public async Task Execute(IJobExecutionContext context)
        {
            // check is there any un-comment contribution
            var checkUncomment = await _appDbContext.Contributions.Where(x => x.IsCoordinatorComment == false).AnyAsync();
            var manager = await _userManager.GetUsersInRoleAsync(Roles.Manager);
            if (checkUncomment)
            {

                foreach (var user in manager)
                {
                    var notCommentContributionUrl = _configuration["ApplicationSettings:NotCommentContributionUrl"];
                    var emailBody = $"There are some contributions which do not have comments. To view more detail, see this link: {notCommentContributionUrl}";
                    _emailService.SendEmail(new MailRequest
                    {
                        ToEmail = user.Email,
                        Body = emailBody,
                        Subject = "COMMENT ON CONTRIBUTION"
                    });
                }

               
            }
            else
            {
                foreach (var user in manager)
                {
                    _emailService.SendEmail(new MailRequest
                    {
                        ToEmail = user.Email,
                        Body = "All contribution has been comment by coordinators",
                        Subject = "COMMENT ON CONTRIBUTION"
                    });
                }
            }
            _logger.LogInformation("Email has sent to manager success ----- {Utc.Now}",DateTime.UtcNow);
          
        }
    }
}
