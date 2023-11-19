using Application.Abstractions;

namespace Application.Github.Queries.Get;

/// <summary>
/// </summary>
public record GetCompaniesQuery : IQuery<List<GetCompaniesResponse>>
{
}