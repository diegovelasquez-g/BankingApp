using BankingApp.Application.Domain.Interfaces;
using BankingApp.Shared.Dtos.Responses;
using FluentValidation;
using MediatR;

namespace BankingApp.Application.Features.CreditCards.Queries;

public class GetUserCreditCardsQuery : IRequest<IEnumerable<CreditCardsResponse>>
{
    public Guid UserId { get; set; }
}

public class GetUserCreditCardsQueryHandler : IRequestHandler<GetUserCreditCardsQuery, IEnumerable<CreditCardsResponse>>
{
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly IPurchaseRepository _purchaseRepository;

    public GetUserCreditCardsQueryHandler(ICreditCardRepository creditCardRepository, IPurchaseRepository purchaseRepository)
    {
        _creditCardRepository = creditCardRepository;
        _purchaseRepository = purchaseRepository;
    }

    public async Task<IEnumerable<CreditCardsResponse>> Handle(GetUserCreditCardsQuery request, CancellationToken cancellationToken)
    {
        var creditCards = await _creditCardRepository.GetCreditCardsByUserIdAsync(request.UserId);
        return creditCards.Select(c => new CreditCardsResponse
        {
            CreditCardId = c.CreditCardId,
            CardHolderName = c.CardHolder,
            CardNumber = c.CardNumber,
            AvailableBalance = c.AvailableBalance,
        });
    }

    public class GetUserCreditCardsQueryValidator : AbstractValidator<GetUserCreditCardsQuery>
    {
        public GetUserCreditCardsQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}