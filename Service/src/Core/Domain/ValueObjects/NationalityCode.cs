using System.Text.RegularExpressions;
using Domain.Errors;
using Domain.Primitives;
using Domain.Primitives.Result;

namespace Domain.ValueObjects;

public class NationalCode : ValueObject
{
    private NationalCode(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<NationalCode> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DomainErrors.Person.EmptyNationalCode;

        value = value.Trim();

        if (value.Length != 10)
            return DomainErrors.Person.WrongLengthNationalCode;

        var regex = new Regex(@"\d{10}");
        if (!regex.IsMatch(value))
            return DomainErrors.Person.DigitOnlyCharacterNationalCode;

        var forbiddenCodes = new[]
        {
            "0000000000", "1111111111", "2222222222",
            "3333333333", "4444444444", "5555555555",
            "6666666666", "7777777777", "8888888888",
            "9999999999", "0123456789", "1234567890"
        };
        if (forbiddenCodes.Contains(value))
            return DomainErrors.Person.UnacceptableNationalCode;


        var chArray = value.ToCharArray();
        var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
        var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
        var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
        var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
        var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
        var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
        var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
        var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
        var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
        var a = Convert.ToInt32(chArray[9].ToString());

        var b = num0 + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9;
        var c = b % 11;

        if ((c < 2 && a == c) || (c >= 2 && 11 - c == a))
            return new NationalCode(value);

        return DomainErrors.Person.InvalidNationalCode;
    }


    public static implicit operator string(NationalCode name)
    {
        return name.Value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}