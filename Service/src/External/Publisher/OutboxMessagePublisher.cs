using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Persistence;
using Persistence.Outbox;
using Quartz;

namespace Publisher;

[DisallowConcurrentExecution]
public class OutboxMessagePublisher : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly ILogger<OutboxMessagePublisher> _logger;

    public OutboxMessagePublisher(
        ApplicationDbContext dbContext,
        IPublisher publisher,
        ILogger<OutboxMessagePublisher> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext.Set<OutboxMessage>().Where(m => m.ProcessedOnUtc == null).Take(20)
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
                _logger.LogError($"Event '{message.Type}' cannot be deserialized.", message);
                continue;
            }

            await _publisher.Publish(domainEvent, context.CancellationToken);
            message.ProcessedOnUtc = DateTime.Now;
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}