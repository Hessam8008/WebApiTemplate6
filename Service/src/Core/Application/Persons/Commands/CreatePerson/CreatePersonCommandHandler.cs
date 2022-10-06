using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Primitives.Result;
using Domain.ValueObjects;

namespace Application.Persons.Commands.CreatePerson;

public sealed class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand>
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
        var firsName = FirstName.Create(command.Name);
        var lastName = LastName.Create(command.Family);
        var email = Email.Create(command.Email);
        var result = Result.Combine(firsName, lastName, email);
        if (result.IsFailure)
            return result;

        var person = Person.Create(firsName, lastName, email);
        await _personRepository.InsertAsync(person);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}