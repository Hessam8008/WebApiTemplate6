using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Presentation.Abstractions;
using Presentation.Models;

namespace Presentation.Controllers;

/// <summary>
///     User actions
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]

// [Route("api/v{version:apiVersion}/[controller]")]
[Route("")]
public class UserController : ApiController
{
    private readonly JwtSettings _jwtSettings;

    /// <summary>
    ///     User controller
    /// </summary>
    /// <param name="sender">MediatR sender</param>
    /// <param name="configuration"></param>
    public UserController(ISender sender, IConfiguration configuration)
        : base(sender) =>
        _jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>() ?? new JwtSettings();

    [HttpGet("Token")]
    [AllowAnonymous]
    public IActionResult GenerateJWT() => Ok(GenerateToken());

    /// <summary>
    ///     User identity information
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult WhoIAm()
    {
        var dic = new
        {
            User.Identity?.Name,
            Claims = User.Claims.Select(x => new { x.Type, x.Value, x.ValueType })
        };

        return Ok(dic);
    }

    private string GenerateToken()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.IssuerSigningKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtClaimTypes.Name, "Demo User"), new Claim(JwtClaimTypes.Role, "DemoUser"),
            new Claim(JwtClaimTypes.Email, "Demo@WebApi.com")
        };
        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.ValidAudience,
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}