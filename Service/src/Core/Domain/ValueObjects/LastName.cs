using Domain.Errors;
using Domain.Primitives;
using Domain.Primitives.Result;

namespace Domain.ValueObjects;

public sealed class LastName : ValueObject
{
    private const int MaxLength = 30;


    private LastName(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<LastName> Create(string? lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<LastName>(DomainErrors.Person.EmptyLastName);

        lastName = lastName.Trim();

        return lastName.Length > MaxLength
            ? Result.Failure<LastName>(DomainErrors.Person.TooLongLastName)
            : new LastName(lastName);
    }


    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator string(LastName value)
    {
        return value.Value;
    }
}