using Domain.Errors;
using Domain.Primitives;
using Domain.Primitives.Result;

namespace Domain.ValueObjects;

public sealed class ContactTitle : ValueObject
{
    public const int MaxLength = 50;

    private ContactTitle(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<ContactTitle> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DomainErrors.General.ValueIsRequired();

        value = value.Trim();

        return value.Length > MaxLength
            ? DomainErrors.General.TooLong()
            : new ContactTitle(value);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator string(ContactTitle value)
    {
        return value.Value;
    }
}