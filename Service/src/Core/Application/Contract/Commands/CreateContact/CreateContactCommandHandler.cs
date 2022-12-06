using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Primitives.Result;
using Domain.ValueObjects;

namespace Application.Contract.Commands.CreateContact;

/// <summary>
///     Handler for create contact.
/// </summary>
public sealed class CreateContactCommandHandler : ICommandHandler<CreateContactCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="unitOfWork"></param>
    public CreateContactCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    ///     Create a contact
    /// </summary>
    /// <param name="command">Create contact command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>If success then return Result.Success else return result object.</returns>
    public async Task<Result> Handle(CreateContactCommand command, CancellationToken cancellationToken)
    {
        var title = ContactTitle.Create(command.Title);
        var caption = ContactCaption.Create(command.Caption);
        var internalNumber = InternalNumber.Create(command.InternalNumber);
        var inputResult = Result.Combine(title, caption, internalNumber);
        if (inputResult.IsFailure)
            return inputResult;

        var existsInternalNumber =
            await _unitOfWork.ContactRepository.ExistsInternalNumber(internalNumber.Value, cancellationToken);

        var contact = Contact.Create(title, caption, internalNumber, command.Building, null, existsInternalNumber);

        if (contact.IsFailure)
            return contact;

        await _unitOfWork.ContactRepository.InsertAsync(contact, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}