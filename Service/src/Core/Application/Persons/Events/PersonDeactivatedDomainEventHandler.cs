using Domain.DomainEvents;
using MediatR;

namespace Application.Persons.Events;

public sealed class PersonDeactivatedDomainEventHandler : INotificationHandler<PersonDeactivatedDomainEvent>
{
    public async Task Handle(PersonDeactivatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Send email
    }
}