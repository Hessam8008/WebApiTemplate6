using Application.Company.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Presentation.Abstractions;

namespace Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("")]
public class CompanyController : ApiController
{
    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    public CompanyController(ISender sender) : base(sender)
    {
    }

    /// <summary>
    ///     Return all cities.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [OutputCache(Duration = 10)]
    public async Task<IActionResult> GetOrgAsync(CancellationToken cancellationToken) =>
        Ok(await Sender.Send(new GetCompaniesQuery(), cancellationToken));
}