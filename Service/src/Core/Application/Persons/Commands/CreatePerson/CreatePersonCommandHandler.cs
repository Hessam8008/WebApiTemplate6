using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Primitives.Result;

namespace Application.Persons.Commands.CreatePerson;

public sealed class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand, Result>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePersonCommandHandler(IPersonRepository personRepository, IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreatePersonCommand command, CancellationToken cancellationToken)
    {
        var person = Person.Create();
        await _personRepository.Insert(person);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}