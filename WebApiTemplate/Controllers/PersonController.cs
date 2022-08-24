using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.Models;

namespace WebApiTemplate.Controllers;

public class PersonController : ControllerBase
{
    [HttpGet("Throw")]
    public IActionResult ThrowEx()
    {
        throw new Exception("System exception.");
    }

    [HttpGet("ThrowHttpEx")]
    public IActionResult ThrowHttpEx()
    {
        throw new HttpResponseException(302, "Http exception.");
    }


    [HttpGet("{id}")]
    public IActionResult GetById(int id = 258)
    {
        return Ok(id);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Post([FromBody] string? data = "Hello world!")
    {
        if (data == null)
            return BadRequest();

        return CreatedAtAction(nameof(GetById), new {id = DateTime.Now.Millisecond}, data);
    }
}