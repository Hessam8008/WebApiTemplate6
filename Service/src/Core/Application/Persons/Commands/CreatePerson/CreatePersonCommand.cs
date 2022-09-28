using Application.Abstractions;
using Domain.Primitives.Result;

namespace Application.Persons.Commands.CreatePerson;

public sealed record CreatePersonCommand(string Name, string Family) : ICommand<Result>;
