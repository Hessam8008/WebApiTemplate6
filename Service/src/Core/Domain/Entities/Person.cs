using Domain.Exceptions;
using Domain.Primitives;

namespace Domain.Entities;

public sealed class Person : Entity
{
    public string? Name { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public string? NationCode { get; private set; }
    public string? Nationality { get; private set; }
    public DateTime CreateTime { get; private set; }

    public void ChangeNation(string newNation)
    {
        if (newNation.Equals("iran"))
            throw new DomainException("Iran is in the black list.", 200).Add("You are not allowed to do this.", 201);

        Nationality = newNation;
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


    public static Person Create()
    {
        return new Person(Guid.NewGuid())
        {
            Name = "Hessam Hosseini ",
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

    protected Person(Guid id) : base(id)
    {
    }
}