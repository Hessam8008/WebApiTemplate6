using Domain.Primitives;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class Person
    {
        public static readonly Error BlockedNationality = new("Person.Nationality", "Nationality has been blocked.");
    }
}