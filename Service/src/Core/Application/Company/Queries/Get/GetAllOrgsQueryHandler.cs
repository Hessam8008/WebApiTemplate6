using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Abstractions;
using Domain.Abstractions;
using Domain.Primitives.Result;

namespace Application.Github.Queries.Get;

public class GetCountriesQueryHandler : IQueryHandler<GetCompaniesQuery, List<GetCompaniesResponse>>
{
    private static readonly string CacheKey = "AllCities";
    private readonly ICacheProvider _cacheProvider;

    /// <summary>
    ///     Constructor for get all cities.
    /// </summary>
    /// <param name="cacheProvider"></param>
    public GetCountriesQueryHandler(ICacheProvider cacheProvider) => _cacheProvider = cacheProvider;

    /// <summary>
    ///     Get all cities with state.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<GetCompaniesResponse>>>
        Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var cache = await _cacheProvider.GetCache<List<GetCompaniesResponse>>(CacheKey);
        if (cache is not null) return cache;

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response =
            await client.GetFromJsonAsync<List<GetCompaniesResponse>>("https://testapi.devtoolsdaily.com/companies/",
                cancellationToken);
        var result = response?.ToList() ?? Enumerable.Empty<GetCompaniesResponse>().ToList();
        await _cacheProvider.AddCache(CacheKey, result);
        return result;
    }
}