using System.Data;

namespace Application.Persons.Queries.GetPersonById;

public sealed class GetPersonByIdQueryHandler : IQueryHandler<GetPersonByIdQuery, PersonResponse>
{
    private readonly IDbConnection _dbConnection;

    public GetPersonByIdQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }


    public async Task<PersonResponse> Handle(GetPersonByIdQuery query, CancellationToken cancellationToken)
    {
        const string sql = @"Select * from dbo.Person where Id = @Id";
        var person = await _dbConnection.QueryFirstOrDefaultAsync<PersonResponse>(sql, new
        {
            query.Id
        });
        if (person is null) throw new Exception("Not found Person.");

        return person;
    }
}