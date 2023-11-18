using Application.Github.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("")]
public class GithubController : ApiController
{
    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    public GithubController(ISender sender) : base(sender)
    {
    }

    /// <summary>
    ///     Return all cities.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetOrgAsync(CancellationToken cancellationToken) =>
        Ok(await Sender.Send(new GetAllOrgsQuery(), cancellationToken));
}