using Domain.Enums;
using Domain.Errors;
using Domain.Primitives;
using Domain.Primitives.Result;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Person : Entity
{
    protected Person()
    {
    }

    private Person(Guid id) : base(id)
    {
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public NationalCode NationCode { get; private set; }
    public string? Nationality { get; private set; }
    public Email Email { get; private set; }
    public DateTime CreateTime { get; private set; }


    public Result ChangeNation(string newNation)
    {
        if (newNation.Equals("iran"))
            return Result.Failure(DomainErrors.Person.BlockedNationality);


        Nationality = newNation;
        return Result.Success();
    }

    public Result ChangeNationCode(NationalCode code)
    {
        NationCode = code;
        return Result.Success();
    }

    public Result SetBirthDate(int y, int m, int d)
    {
        try
        {
            BirthDate = new DateOnly(y, m, d);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(DomainErrors.Person.InvalidBirthDay);
        }
    }

    public static Person Create(FirstName firstName, LastName lastName, Email email)
    {
        return new Person(Guid.NewGuid())
        {
            FirstName = firstName,
            LastName = lastName,
            BirthDate = new DateOnly(1985, 11, 21),
            Gender = Gender.Male,
            NationCode = NationalCode.Create("0946507767"),
            Nationality = "Germany",
            Email = email,
            CreateTime = DateTime.Now
        };
    }
}