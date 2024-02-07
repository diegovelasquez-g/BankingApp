using BankingApp.Application.Features.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("myTransactions")]
    public async Task<IActionResult> GetUserTransactions(Guid userId)
    {
        var query = new GetUserTransactionsQuery { UserId = userId };
        var response = await _mediator.Send(query);
        return Ok(response);
    }
}
