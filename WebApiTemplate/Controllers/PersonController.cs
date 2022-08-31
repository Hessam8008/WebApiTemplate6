using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.Domain.Models;
using static WebApiTemplate.Controllers.ErrorHandlingController;

namespace WebApiTemplate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    [HttpGet("SystemException")]
    [ProducesResponseType(typeof(HttpExceptionResponse), StatusCodes.Status500InternalServerError)]
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
    [ProducesResponseType(typeof(HttpExceptionResponse), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(HttpDomainErrorResponse), ExtraStatusCodes.Status499DomainError)]
    [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
    public IActionResult DomainException()
    {
        var p = Person.Create();
        p.ChangeNation("iran"); // Domain exception raise here
        return Ok(p);
    }


    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetById(int id = 258)
    {
        return Ok();
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Post([FromBody] string? data = "Hello world!")
    {
        if (data == null)
            return BadRequest();

        return CreatedAtAction(nameof(GetById), new {id = DateTime.Now.Millisecond}, new {data});
    }
}