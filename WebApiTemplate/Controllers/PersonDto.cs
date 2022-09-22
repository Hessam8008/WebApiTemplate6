using WebApiTemplate.Domain.Models;

namespace WebApiTemplate.Controllers;

public record PersonDto()
{
    public int Id { get; }
    public string Name { get; }

    public DateOnly BirthDate { get; }
    public Gender Gender { get; }
    public string NationCode { get; }
    public string Nationality { get; }
    public DateTime CreateTime { get; }

    public PersonDto(Person person) : this()
    {
        Id = person.Id;
        Name = person.Name;
        BirthDate = person.BirthDate;
        Gender = person.Gender;
        NationCode = person.NationCode;
        Nationality = person.Nationality;
        CreateTime = person.CreateTime;
    }
}