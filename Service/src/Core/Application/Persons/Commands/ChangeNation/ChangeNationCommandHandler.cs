using Application.Abstractions;
using Domain.Abstractions;
using Domain.Primitives;
using Domain.Primitives.Result;

namespace Application.Persons.Commands.ChangeNation;

public class ChangeNationCommandHandler : ICommandHandler<ChangeNationCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonRepository _personRepository;

    public ChangeNationCommandHandler(IUnitOfWork unitOfWork, IPersonRepository personRepository)
    {
        _unitOfWork = unitOfWork;
        _personRepository = personRepository;
    }

    public async Task<Result> Handle(ChangeNationCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.SelectAsync(request.Id);
        if (person is null)
            return Result.Failure(new Error("Person.NotFound", "Person not found"));

        person.ChangeNation(request.Nation);

        await _personRepository.UpdateAsync(person);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}