using Domain.Errors;
using Domain.Primitives;
using Domain.Primitives.Result;

namespace Domain.ValueObjects;

public class InternalNumber : ValueObject
{
    private InternalNumber(short value)
    {
        Value = value;
    }

    public short Value { get; }

    public static Result<InternalNumber> Create(short value)
    {
        if (value is 0 or >= 100 and <= 900)
            return new InternalNumber(value);
        return DomainErrors.General.ValueIsInvalid(nameof(InternalNumber), value.ToString());
    }

    public static implicit operator short(InternalNumber value)
    {
        return value?.Value ?? 0;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}