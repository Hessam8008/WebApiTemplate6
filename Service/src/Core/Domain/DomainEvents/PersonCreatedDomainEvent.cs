using Domain.Primitives;

namespace Domain.DomainEvents;

public sealed record PersonCreatedDomainEvent(Guid PersonId) : IDomainEvent;