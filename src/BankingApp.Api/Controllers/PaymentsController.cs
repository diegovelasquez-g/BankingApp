using BankingApp.Application.Features.Payments.Commands;
using BankingApp.Application.Features.Payments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("newPayment")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand command)
    {
        await _mediator.Send(command);
        return Ok("The payment has been saved.");
    }

    [HttpGet("myPayments")]
    public async Task<IActionResult> GetUserPayments(Guid userId)
    {
        var query = new GetUserPaymentsQuery { UserId = userId };
        var response = await _mediator.Send(query);
        return Ok(response);
    }

}
