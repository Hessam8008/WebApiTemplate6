using Domain.Errors;
using Domain.Primitives;
using Domain.Primitives.Result;

namespace Domain.ValueObjects;

public class InternalNumber : ValueObject
{
    public InternalNumber(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<InternalNumber> Create(int value)
    {
        if (value is 0 or >= 100 and <= 900)
            return new InternalNumber(value);
        return DomainErrors.General.ValueIsInvalid(nameof(InternalNumber), value.ToString());
    }

    public static implicit operator int(InternalNumber value)
    {
        return value.Value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}