using Microsoft.Extensions.Options;
using Quartz;

namespace Server.Infrastructure.Jobs.JobSetup
{
    public class JobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            // mail job
            var mailManagerJobKey = JobKey.Create(nameof(MailManagerJob));
            options.AddJob<MailManagerJob>(jobBuilder => jobBuilder.WithIdentity(mailManagerJobKey))
                .AddTrigger(trigger => trigger.ForJob(mailManagerJobKey).WithSimpleSchedule(schedule => schedule.WithIntervalInHours(24).RepeatForever()));
            // academic year job
            var checkAcademicYearJobKey = JobKey.Create(nameof(CheckAcademicYearJob));
            options.AddJob<CheckAcademicYearJob>(jobBuilder => jobBuilder.WithIdentity(checkAcademicYearJobKey))
                .AddTrigger(trigger =>
                    trigger.ForJob(checkAcademicYearJobKey)
                        .WithSimpleSchedule(schedule => schedule.WithIntervalInHours(24).RepeatForever()));
        }
    }
}
