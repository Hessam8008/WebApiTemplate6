using Application.Abstractions;

namespace Application.Company.Queries.Get;

/// <summary>
/// </summary>
public record GetCompaniesQuery : IQuery<List<GetCompaniesResponse>>
{
}