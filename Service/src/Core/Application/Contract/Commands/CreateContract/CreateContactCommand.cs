using Application.Abstractions;
using Domain.Enums;

namespace Application.Contract.Commands.CreateContract;

/// <summary>
///     Command to create a contact.
/// </summary>
public sealed record CreateContactCommand : ICommand
{
    public string Title { get; set; }

    public string Caption { get; set; }

    public int InternalNumber { get; set; }

    public Building Building { get; set; }


}