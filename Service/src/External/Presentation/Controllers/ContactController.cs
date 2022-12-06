using Application.Contract.Commands.CreateContact;
using Application.Contract.Queries.Get;
using Application.Contract.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Controllers;

/// <summary>
///     All methods to handle person affairs.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("")]
public class ContactController : ApiController
{
    /// <summary>
    ///     Constructor for the controller
    /// </summary>
    /// <param name="sender"></param>
    public ContactController(ISender sender) : base(sender)
    {
    }

    /// <summary>
    ///     Register new contact.
    /// </summary>
    /// <param name="command">Data for create contact.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateContactAsync(
        [FromBody] CreateContactCommand command,
        CancellationToken cancellationToken)
    {
        return CreatedAtAction("GetContact", await Sender.Send(command, cancellationToken));
    }

    /// <summary>
    ///     Get all contacts include inactive contacts.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAllContacts(CancellationToken cancellationToken)
    {
        return Ok(await Sender.Send(new GetAllContactQuery(), cancellationToken));
    }

    /// <summary>
    ///     Gets the contact by id.
    /// </summary>
    /// <param name="id">The contact identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetContact([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(await Sender.Send(new GetContactQuery(id), cancellationToken));
    }

    /// <summary>
    ///     Throw an exception for handling errors.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpGet("error")]
    public Task<IActionResult> ThrowException()
    {
        throw new Exception("This is a an exception for test error handling.");
    }
}