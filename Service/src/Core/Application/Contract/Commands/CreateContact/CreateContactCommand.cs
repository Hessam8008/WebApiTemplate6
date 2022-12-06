using Application.Abstractions;
using Domain.Enums;

namespace Application.Contract.Commands.CreateContact;

/// <summary>
///     Command to create a contact.
/// </summary>
public sealed record CreateContactCommand : ICommand
{
    /// <summary>
    ///     Title of the contact.
    /// </summary>
    /// <example>Mr. William</example>
    public string Title { get; set; }

    /// <summary>
    ///     Caption of the contact. It displays below the title.
    /// </summary>
    /// <example>CTO</example>
    public string Caption { get; set; }

    /// <summary>
    ///     The internal phone number in telephone central.
    /// </summary>
    /// <example>147</example>
    public short InternalNumber { get; set; }

    /// <summary>
    ///     Where the contact located is.
    /// </summary>
    public Building Building { get; set; }
}