using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Contracts.Common;
using Server.Domain.Common.Constants;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;


namespace Server.Infrastructure.Jobs
{
    [DisallowConcurrentExecution]
    public class CheckAcademicYearJob : IJob
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<CheckAcademicYearJob> _logger;
        private readonly IEmailService _emailService;
        public CheckAcademicYearJob(AppDbContext appDbContext,IUnitOfWork unitOfWork,ILogger<CheckAcademicYearJob> logger, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
            _emailService = emailService;

        }
        public async Task Execute(IJobExecutionContext context)
        {
            var date = DateTime.UtcNow;
            const string expiredRejectNote = "Academic Year is expired, your contribution is rejected";
            var currentAcademicYear = await _unitOfWork.AcademicYearRepository.GetAcademicYearByCurrentYearAsync(date);
            if (currentAcademicYear.FinalClosureDate < date)
            {
                var pendingContributionInCurrentYear =  _unitOfWork.ContributionRepository.Find(x => x.AcademicYearId == currentAcademicYear.Id && x.Status == ContributionStatus.Pending).ToList();
                foreach(var item in pendingContributionInCurrentYear)
                {
                    var manager = await _userManager.GetUsersInRoleAsync(Roles.Admin);
                    foreach(var user in manager)
                    {
                        await _unitOfWork.ContributionRepository.Reject(item, user.Id, expiredRejectNote);
                        var student = await _userManager.FindByIdAsync(item.UserId.ToString());
                        var faculty = await _unitOfWork.FacultyRepository.GetByIdAsync((Guid)student?.FacultyId!);
                        _emailService.SendEmail(new MailRequest
                        {
                            ToEmail = student.Email,
                            Body = $"<div style=\"font-family: Arial, sans-serif; color: #800080; padding: 20px;\">\r\n " +
                                   $" <h2>Coordinator rejected your contribution with reason: {expiredRejectNote}</h2>\r\n " +
                                   $" <p style=\"margin: 5px 0; font-size: 18px;\">Blog Title: {item.Title} 2</p>\r\n " +
                                   $" <p style=\"margin: 5px 0; font-size: 18px;\">Content: {item.Content}</p>\r\n" +
                                   $"  <p style=\"margin: 5px 0; font-size: 18px;\">User: {student.UserName}</p>\r\n " +
                                   $"  <p style=\"margin: 5px 0; font-size: 18px;\">Faculty: {faculty.Name}</p>\r\n " +
                                   $" <p style=\"margin: 5px 0; font-size: 18px;\">Academic Year: {currentAcademicYear.Name}</p>\r\n</div>",
                            Subject = "REJECTED CONTRIBUTION"
                        });
                    }
                }
                await _unitOfWork.CompleteAsync();

            }
            _logger.LogInformation("Check Academic Year Job ----- {Utc.Now}", DateTime.UtcNow);


        }
    }
}
