using Application.Persons.Commands.CreatePerson;
using Application.Persons.Queries.GetPersonById;
using Domain.Entities;
using Domain.Primitives.Result;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ApiController
{
    public PersonController(ISender sender) : base(sender)
    {
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterPerson(
        [FromBody] CreatePersonCommand command,
        CancellationToken cancellationToken)
    {
        return Output(await Sender.Send(command, cancellationToken));
    }

    private static Result<Person> CreatePerson()
    {
        var firstName = FirstName.Create("Hessam");
        var lastName = LastName.Create("Hosseini");
        var email = Email.Create("Hessam8008@yahoo.com");
        var nationCode = NationalCode.Create("0946507768");

        var result = Result.Combine(firstName, lastName, email, nationCode);
        if (result.IsFailure)
            return Result.Failure<Person>(result.Error);

        var p = Person.Create(firstName, lastName, email);
        return Result.Success(p);
    }

    [HttpGet("first")]
    [ProducesResponseType(typeof(Result<PersonResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPersonAsync(CancellationToken cancellationToken)
    {
        return Output(await Sender.Send(new GetPersonByIdQuery(Guid.Empty), cancellationToken));
    }


    [HttpGet("SystemException")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public IActionResult SystemException()
    {
        var result = CreatePerson();
        if (result.IsFailure)
            return BadRequest(result);

        var person = result.Value;
        var k = person.SetBirthDate(2022, 13, 32);
        return Ok(k);
    }

    [HttpPut("DomainException")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public IActionResult DomainException()
    {
        var result = CreatePerson();
        if (result.IsFailure)
            return BadRequest(result);

        var person = result.Value;
        var k = person.ChangeNation("iran");
        return Ok(k);
    }

    [HttpPut("DomainException2")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public IActionResult DomainException2()
    {
        var nationCode = NationalCode.Create("0946507768");

        var result = CreatePerson();
        var person = result.Value;
        var k = person.ChangeNationCode(nationCode);
        return Ok(k);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
    public IActionResult GetById(int id = 258)
    {
        if (id < 100)
            return NotFound();

        var firstName = FirstName.Create("Hessam");
        var lastName = LastName.Create("Hosseini");
        var email = Email.Create("Hessam8008@yahoo.com");
        var nationCode = NationalCode.Create("0946507768");

        var result = Result.Combine(firstName, lastName, email, nationCode);
        if (result.IsFailure)
            return BadRequest(result);
        var p = Person.Create(firstName, lastName, email);

        return Ok(new PersonDto(p));
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Post([FromBody] string? data = "Hello world!")
    {
        if (data == null)
            return BadRequest();

        return CreatedAtAction(nameof(GetById), new {id = DateTime.Now.Millisecond}, new {data});
    }
}