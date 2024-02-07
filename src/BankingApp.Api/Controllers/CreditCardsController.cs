using BankingApp.Application.Features.CreditCards.Queries;
using BankingApp.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CreditCardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CreditCardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("myCreditCardAccountStatus")]
    public async Task<IActionResult> GetUserCreditCardAccountStatus(Guid creditCardId)
    {
        var query = new GetUserCreditCardAccountStatusQuery { CreditCardId = creditCardId };
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("downloadCreditCardStatus")]
    public async Task<IActionResult> DownloadCreditCardStatus(Guid creditCardId)
    {
        var query = new GetUserCreditCardAccountStatusQuery { CreditCardId = creditCardId };
        var response = await _mediator.Send(query);

        GeneratePDF generatePDF = new GeneratePDF();
        var stream = generatePDF.GenerateCreditCardStatus(response);
        if (stream == null)
        {
            return NotFound();
        }
        return File(stream, "application/pdf", "Estado-de-Cuenta.pdf");
    }

    [HttpGet("myCreditCards")]
    public async Task<IActionResult> GetUserCreditCards(Guid userId)
    {
        var query = new GetUserCreditCardsQuery { UserId = userId };
        var response = await _mediator.Send(query);
        return Ok(response);
    }
}