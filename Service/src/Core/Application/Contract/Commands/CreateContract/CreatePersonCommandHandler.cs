using Application.Abstractions;
using Application.Contract.Commands.CreateContract;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Primitives.Result;
using Domain.ValueObjects;

namespace Application.Persons.Commands.CreatePerson;

public sealed class CreateContactCommandHandler : ICommandHandler<CreateContactCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateContactCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateContactCommand command, CancellationToken cancellationToken)
    {
        var title = ContactTitle.Create(command.Title);
        var caption = ContactCaption.Create(command.Caption);
        var internalNumber = InternalNumber.Create(command.InternalNumber);
        var result = Result.Combine(title, caption, internalNumber);
        if (result.IsFailure)
            return result;

        var contact = Contact.Create(title, caption, internalNumber, command.Building);
        await _unitOfWork.ContactRepository.InsertAsync(contact, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}