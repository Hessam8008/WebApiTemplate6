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


    public Result<LastName> Create(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<LastName>(new Error("LastName.Empty", "Last name is empty."));

        return firstName.Length > MaxLength
            ? Result.Failure<LastName>(new Error("LastName.MaxLength", "Last name is too long."))
            : new LastName(firstName);
    }


    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}