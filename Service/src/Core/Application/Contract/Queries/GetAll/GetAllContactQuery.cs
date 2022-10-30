using Application.Abstractions;

namespace Application.Contract.Queries.GetAll;

public sealed record GetAllContactQuery : IQuery<List<ContactResponse>>;