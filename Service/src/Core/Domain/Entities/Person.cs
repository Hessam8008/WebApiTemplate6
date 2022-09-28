using Domain.Enums;
using Domain.Errors;
using Domain.Exceptions;
using Domain.Primitives;
using Domain.Primitives.Result;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Person : Entity
{
    protected Person(Guid id) : base(id)
    {
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public string? NationCode { get; private set; }
    public string? Nationality { get; private set; }
    public DateTime CreateTime { get; private set; }

    public Result ChangeNation(string newNation)
    {
        if (newNation.Equals("iran"))
            return Result.Failure(DomainErrors.Person.BlockedNationality);


        Nationality = newNation;
        return Result.Success();
    }

    public void ChangeNationCode(string code)
    {
        var ex = new DomainException();

        if (code.StartsWith("000"))
            ex.Add("New code is incorrect.", 100);

        if (code.Length != 10)
            ex.Add("10 digit required for NationCode.", 101);

        ex.ThrowIfNeeded();

        NationCode = code;
    }


    public static Person Create(FirstName firstName, LastName lastName)
    {
        return new Person(Guid.NewGuid())
        {
            FirstName = firstName,
            LastName = lastName,
            BirthDate = new DateOnly(1985, 11, 21),
            Gender = Gender.Male,
            NationCode = "0946507767",
            Nationality = "Germany",
            CreateTime = DateTime.Now
        };
    }

    public void SetBirthDate(int y, int m, int d)
    {
        BirthDate = new DateOnly(y, m, d);
    }
}