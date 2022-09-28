using Application.Abstractions;
using Domain.Primitives.Result;

namespace Application.Persons.Commands.ChangeNation;

public class ChangeNationCommandHandler : ICommandHandler<ChangeNationCommand, Result>
{
    public Task<Result> Handle(ChangeNationCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}