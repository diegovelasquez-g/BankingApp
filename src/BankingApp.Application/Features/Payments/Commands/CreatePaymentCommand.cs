using BankingApp.Application.Domain.Entities;
using BankingApp.Application.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace BankingApp.Application.Features.Payments.Commands;

public class CreatePaymentCommand : IRequest<Unit>
{
    public Guid CreditCardId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Unit>
{
    private readonly IPaymentRepository _paymentRepository;

    public CreatePaymentCommandHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Unit> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid(),
            CreditCardId = request.CreditCardId,
            Amount = request.Amount,
            Date = request.Date
        };

        await _paymentRepository.AddAsync(payment);
        return Unit.Value;
    }

    public class CreatePurchaseValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePurchaseValidator()
        {
            RuleFor(x => x.CreditCardId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Date).NotEmpty();
        }
    }
}