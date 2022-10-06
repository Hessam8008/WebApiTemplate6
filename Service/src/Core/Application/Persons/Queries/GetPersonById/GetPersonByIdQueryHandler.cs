using Application.Abstractions;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Primitives.Result;

namespace Application.Persons.Queries.GetPersonById;

public sealed class GetPersonByIdQueryHandler : IQueryHandler<GetPersonByIdQuery, PersonResponse>
{
    private readonly IPersonRepository _repository;

    public GetPersonByIdQueryHandler(IPersonRepository repository)
    {
        _repository = repository;
    }


    public async Task<Result<PersonResponse>> Handle(GetPersonByIdQuery query, CancellationToken cancellationToken)
    {
        const string sql = @"Select * from dbo.Person where Id = @Id";
        var person = await _repository.SelectAsync(query.Id);

        return person is null
            ? Result.Failure<PersonResponse>(DomainErrors.General.RecordNotFound())
            : new PersonResponse();
    }
}