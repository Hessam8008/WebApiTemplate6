using Domain.Entities;
using Domain.Primitives;

namespace Domain.DomainEvents;

public sealed record ContactCreated(Contact Contact) : IDomainEvent;