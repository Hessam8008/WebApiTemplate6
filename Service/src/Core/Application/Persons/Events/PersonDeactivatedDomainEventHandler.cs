using System.Diagnostics;
using Domain.DomainEvents;
using MediatR;

namespace Application.Persons.Events;

public sealed class PersonDeactivatedDomainEventHandler : INotificationHandler<PersonCreatedDomainEvent>
{
    public async Task Handle(PersonCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Debug.WriteLine("Sending email...");
        await Task.CompletedTask;
        // Send email
    }
}