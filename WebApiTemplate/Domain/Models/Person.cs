namespace WebApiTemplate.Domain.Models;

public class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? NationCode { get; set; }
    public string? Nationality { get; set; }


    public void ChangeNation(string newNation)
    {
        if (newNation.Equals("iran"))
            throw new DomainException("Iran is in black list.");
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