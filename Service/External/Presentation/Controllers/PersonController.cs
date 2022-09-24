using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    [HttpGet("SystemException")]
    public IActionResult SystemException()
    {
        var p = new Person
        {
            Name = @"Bob"
        };
        p.SetBirthDate(2022, 13, 32);
        return Ok(p);
    }

    [HttpPut("DomainException")]
    [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
    public IActionResult DomainException()
    {
        var p = Person.Create();
        p.ChangeNation("iran"); // Domain exception raise here
        return Ok(p);
    }

    [HttpPut("DomainException2")]
    [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
    public IActionResult DomainException2()
    {
        var p = Person.Create();
        p.ChangeNationCode("000258"); // Domain exception raise here
        return Ok(p);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
    public IActionResult GetById(int id = 258)
    {
        if (id < 100)
            return NotFound();
        return Ok(new PersonDto(Person.Create()));
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