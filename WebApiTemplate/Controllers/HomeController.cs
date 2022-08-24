using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTemplate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    [HttpGet("Throw")]
    public IActionResult ThrowEx()
    {
        throw new Exception("System exception.");
    }

    [HttpGet("ThrowHttpEx")]
    public IActionResult ThrowHttpEx()
    {
        throw new HttpResponseException(401, "Http exception.");
    }


    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleError()
    {
        return Problem();
    }


    [Route("/error-development")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleErrorDevelopment(
        [FromServices] IHostEnvironment hostEnvironment)
    {
        if (!hostEnvironment.IsDevelopment()) return NotFound();

        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        return Problem(
            exceptionHandlerFeature.Error.StackTrace,
            title: exceptionHandlerFeature.Error.Message);
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