using Application.Abstractions;

namespace Application.Contract.Queries.Get;

public sealed record GetContactQuery(Guid Id) : IQuery<ContactResponse>;