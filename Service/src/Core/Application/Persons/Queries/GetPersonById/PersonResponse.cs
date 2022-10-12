namespace Application.Persons.Queries.GetPersonById;

public sealed record PersonResponse(Guid Id, string FirsName, string LastName, string Email);