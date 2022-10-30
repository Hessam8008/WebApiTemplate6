using Application.Abstractions;

namespace Application.Persons.Commands.CreatePerson;

/// <summary>
///     Command to create a person.
/// </summary>
public sealed record CreatePersonCommand : ICommand
{
    /// <summary>
    ///     Name of the person.
    /// </summary>
    /// <example>John</example>
    public string? Name { get; set; }

    /// <summary>
    ///     Family of the person.
    /// </summary>
    /// <example>Wilson</example>
    public string? Family { get; set; }


    /// <summary>
    ///     Email of the person.
    /// </summary>
    /// <example>jwilson@gmail.com</example>
    public string? Email { get; set; }
}