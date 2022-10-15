using Domain.Primitives;

namespace Domain.DomainEvents;

public sealed record PersonDeactivatedDomainEvent(Guid PersonId) : IDomainEvent
{
}