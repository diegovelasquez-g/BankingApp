using BankingApp.Application.Features.Users.Commands;
using BankingApp.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("signIn")]
    public async Task<IActionResult> AuthUser([FromBody] AuthUserQuery query)
    {
        var response = await _mediator.Send(query);
        return Ok(response);
    }
}
