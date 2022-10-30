using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.DomainEvents;

public sealed record ContactCaptionChanged(ContactCaption OldCaption, ContactCaption NewCaption) : IDomainEvent;