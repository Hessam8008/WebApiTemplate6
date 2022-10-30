using Application.Abstractions;
using Domain.Abstractions;
using Domain.Primitives.Result;

namespace Application.Contract.Queries.GetAll;

/// <summary>
///     Class for make response to get all contacts.
/// </summary>
public sealed class GetAllContactQueryHandler : IQueryHandler<GetAllContactQuery, List<ContactResponse>>
{
    private readonly IContactRepository _contactRepository;

    /// <summary>
    ///     Constructor of the GetAllQueryHandler.
    /// </summary>
    /// <param name="contactRepository"></param>
    public GetAllContactQueryHandler(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    /// <summary>
    ///     Handle the query.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<ContactResponse>>> Handle(GetAllContactQuery query,
        CancellationToken cancellationToken)
    {
        var contacts = await _contactRepository.SelectAllAsync(cancellationToken);
        var result = from contact in contacts select new ContactResponse(contact);
        return result.ToList();
    }
}