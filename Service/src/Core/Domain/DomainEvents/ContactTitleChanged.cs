using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.DomainEvents;

public sealed record ContactTitleChanged(ContactTitle OldTitle, ContactTitle NewTitle) : IDomainEvent;