using Domain.Errors;
using Domain.Primitives;
using Domain.Primitives.Result;

namespace Domain.ValueObjects;

public sealed class FirstName : ValueObject
{
    private const int MaxLength = 20;


    private FirstName(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<FirstName> Create(string? firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return DomainErrors.Person.EmptyFirstName;

        firstName = firstName.Trim();

        return firstName.Length > MaxLength
            ? DomainErrors.Person.TooLongFirstName
            : new FirstName(firstName);
    }


    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator string(FirstName value)
    {
        return value.Value;
    }
}