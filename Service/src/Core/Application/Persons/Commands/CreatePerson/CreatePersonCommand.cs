using System.ComponentModel;
using Application.Abstractions;

namespace Application.Persons.Commands.CreatePerson;

/// <summary>
///     Command for create a person.
/// </summary>
/// <seealso cref="Application.Abstractions.ICommand" />
/// <seealso cref="MediatR.IRequest&lt;Domain.Primitives.Result.Result&gt;" />
/// <seealso cref="MediatR.IBaseRequest" />
/// <seealso cref="System.IEquatable&lt;Application.Persons.Commands.CreatePerson.CreatePersonCommand&gt;" />
public sealed record CreatePersonCommand(
        [DefaultValue("Hessam")] string Name = "Hessam",
        string Family = "Hosseini",
        string Email = "example@domain.com")
    : ICommand;
