using BankingApp.Application.Domain.Entities;
using BankingApp.Application.Domain.Interfaces;
using BankingApp.Application.Features.Users.Commands;
using FluentValidation;
using MediatR;

namespace BankingApp.Application.Features.Purchases.Commands;

public class CreatePurchaseCommand : IRequest<Unit>
{
    public Guid CreditCardId { get; set; }
    public string Description { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}

public class CreatePurchaseCommandHandler : IRequestHandler<CreatePurchaseCommand, Unit>
{
    private readonly IPurchaseRepository _purchaseRepository;

    public CreatePurchaseCommandHandler(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<Unit> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = new Purchase
        {
            PurchaseId = Guid.NewGuid(),
            CreditCardId = request.CreditCardId,
            Description = request.Description,
            Amount = request.Amount,
            Date = request.Date
        };

        await _purchaseRepository.AddAsync(purchase);
        return Unit.Value;
    }

    public class CreatePurchaseValidator : AbstractValidator<CreatePurchaseCommand>
    {
        public CreatePurchaseValidator()
        {
            RuleFor(x => x.CreditCardId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Date).NotEmpty();
        }
    }
}
