using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
//[Route("api/v{version:apiVersion}/[controller]")]
[Route("")]
public class UserController : ApiController
{
    public UserController(ISender sender) : base(sender)
    {
    }


    [HttpGet]
    public IActionResult WhoIAm()
    {
        var dic = new
        {
            User.Identity?.Name,
            Claims = User.Claims.Select(x => new {x.Type, x.Value, x.ValueType})
        };

        return Ok(dic);
    }
}