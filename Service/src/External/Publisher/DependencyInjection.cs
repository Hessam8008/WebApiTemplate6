using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Publisher;

public static class DependencyInjection
{
    public static IServiceCollection AddOutboxMessagePublisher(this IServiceCollection services)
    {
        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(OutboxMessagePublisher));
            config.AddJob<OutboxMessagePublisher>(jobKey)
                .AddTrigger(
                    trigger => trigger.ForJob(jobKey)
                        .WithSimpleSchedule(sch =>
                            sch.WithIntervalInSeconds(10)
                                .RepeatForever()));
            config.UseMicrosoftDependencyInjectionJobFactory();
        });
        services.AddQuartzHostedService();
        return services;
    }
}