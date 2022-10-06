using System.Text.RegularExpressions;
using Domain.Errors;
using Domain.Primitives;
using Domain.Primitives.Result;

namespace Domain.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; }


    private Email(string value)
    {
        Value = value;
    }


    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return DomainErrors.General.ValueIsRequired();

        email = email.Trim();

        if (email.Length > 150)
            return DomainErrors.General.InvalidLength();

        if (Regex.IsMatch(email, @"^(.+)@(.+)$") == false)
            return DomainErrors.General.ValueIsInvalid();

        return new Email(email);
    }


    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}