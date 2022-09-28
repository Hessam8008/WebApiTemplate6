using Application.Abstractions;
using Domain.Primitives.Result;

namespace Application.Persons.Commands.ChangeNation;

public record ChangeNationCommand(Guid Id, string Nation) : ICommand<Result>;