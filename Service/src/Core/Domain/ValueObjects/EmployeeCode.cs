using Domain.Errors;
using Domain.Primitives;
using Domain.Primitives.Result;

namespace Domain.ValueObjects;

public class EmployeeCode : ValueObject
{
    public EmployeeCode(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<EmployeeCode> Create(int value)
    {
        if (value is >= 100 and <= 900)
            return new EmployeeCode(value);
        return DomainErrors.General.ValueIsInvalid(nameof(EmployeeCode), value.ToString());
    }

    public static implicit operator int(EmployeeCode? value)
    {
        return value?.Value ?? 0;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}