using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
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
            User.Identity.Name,
            Claims = User.Claims.Select(x => new {x.Type, x.Value, x.ValueType})
        };

        return Ok(dic);
    }
}