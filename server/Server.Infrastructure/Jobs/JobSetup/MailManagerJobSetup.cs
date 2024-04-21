using Microsoft.Extensions.Options;
using Quartz;

namespace Server.Infrastructure.Jobs.JobSetup
{
    public class MailManagerJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(MailManagerJob));
            options.AddJob<MailManagerJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(trigger => trigger.ForJob(jobKey).WithSimpleSchedule(schedule => schedule.WithIntervalInHours(24).RepeatForever()));
        }
    }
}
