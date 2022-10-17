using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Persistence;
using Persistence.Outbox;
using Quartz;
using Serilog;

namespace Publisher;

[DisallowConcurrentExecution]
// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class OutboxMessagePublisher : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;

    public OutboxMessagePublisher(
        ApplicationDbContext dbContext,
        IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext.Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        if (!messages.Any())
            return;

        foreach (var message in messages)
        {
            var domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(message.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent is null)
            {
                var eventType = message.Type;
                Log.ForContext<OutboxMessagePublisher>()
                    .ForContext("event", message)
                    .Error("Event type '{eventType}' cannot be deserialized.", eventType);
                continue;
            }

            await _publisher.Publish(domainEvent, context.CancellationToken);
            message.ProcessedOnUtc = DateTime.Now;
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}