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


    public Result<FirstName> Create(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<FirstName>(new Error("FirstName.Empty", "First name is empty."));

        return firstName.Length > MaxLength
            ? Result.Failure<FirstName>(new Error("FirstName.MaxLength", "First name is too long."))
            : new FirstName(firstName);
    }


    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}