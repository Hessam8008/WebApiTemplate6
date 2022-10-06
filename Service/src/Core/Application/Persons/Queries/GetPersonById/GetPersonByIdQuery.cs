using Application.Abstractions;

namespace Application.Persons.Queries.GetPersonById;

public sealed record GetPersonByIdQuery(Guid Id) : IQuery<PersonResponse>;