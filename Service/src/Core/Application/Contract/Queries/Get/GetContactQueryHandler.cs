using Application.Abstractions;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Primitives.Result;

namespace Application.Contract.Queries.Get;

public sealed class GetContactQueryHandler : IQueryHandler<GetContactQuery, ContactResponse>
{
    private readonly IContactRepository _repository;

    public GetContactQueryHandler(IContactRepository repository)
    {
        _repository = repository;
    }


    public async Task<Result<ContactResponse>> Handle(GetContactQuery query, CancellationToken cancellationToken)
    {
        //const string sql = @"Select * from dbo.Person where Id = @Id";
        var contact = await _repository.SelectAsync(query.Id, cancellationToken);

        return contact is null
            ? Result.Failure<ContactResponse>(DomainErrors.General.RecordNotFound())
            : new ContactResponse(contact);
    }
}