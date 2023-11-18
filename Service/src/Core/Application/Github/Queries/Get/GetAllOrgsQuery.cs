using Application.Abstractions;

namespace Application.Github.Queries.Get;

/// <summary>
/// </summary>
public record GetAllOrgsQuery : IQuery<List<GetOrgResponse>>
{
}