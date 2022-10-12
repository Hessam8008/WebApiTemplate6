using Domain.Primitives.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Abstractions;

public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;


    protected ApiController(ISender sender)
    {
        Sender = sender;
    }

    protected ActionResult Output(Result result)
    {
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result.Error);
    }
}