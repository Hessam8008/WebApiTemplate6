namespace WebApiTemplate.Domain.Models;

public class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? NationCode { get; set; }
    public string? Nationality { get; set; }
    public DateTime CreateTime { get; set; }

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
        return new Person
        {
            Id = DateTime.Now.GetHashCode(),
            Name = "Hessam Hosseini ",
            BirthDate = new DateOnly(1985, 11, 21),
            Gender = Gender.Male,
            NationCode = "0946507767",
            Nationality = "Germany"
        };
    }

    public void SetBirthDate(int y, int m, int d)
    {
        BirthDate = new DateOnly(y, m, d);
    }
}