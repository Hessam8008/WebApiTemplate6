using Application.Abstractions;

namespace Application.Persons.Commands.CreatePerson;

public sealed record CreatePersonCommand(
        string Name = "Hessam",
        string Family = "Hosseini",
        string Email = "example@domain.com")
    : ICommand;
