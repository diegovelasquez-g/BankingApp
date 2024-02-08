using BankingApp.Application.Features.Purchases.Commands;
using BankingApp.Application.Features.Purchases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PurchasesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchasesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("newPurchase")]
    public async Task<IActionResult> CreatePurchase([FromBody] CreatePurchaseCommand command)
    {
        await _mediator.Send(command);
        return Ok("The purchase has been saved.");
    }

    [HttpGet("myPurchases")]
    public async Task<IActionResult> GetUserPurchases(Guid userId)
    {   
        var query = new GetUserPurchasesQuery { UserId = userId };
        var response = await _mediator.Send(query);
        return Ok(response);
    }
}
