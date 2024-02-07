using BankingApp.Application.Domain.Interfaces;
using BankingApp.Shared.Dtos.Responses;
using FluentValidation;
using MediatR;

namespace BankingApp.Application.Features.Purchases.Queries;

public class GetUserPurchasesQuery : IRequest<IEnumerable<PurchasesResponse>>
{
    public Guid UserId { get; set; }
}

public class GetUserPurchasesQueryHandler : IRequestHandler<GetUserPurchasesQuery, IEnumerable<PurchasesResponse>>
{
    private readonly IPurchaseRepository _purchaseRepository;

    public GetUserPurchasesQueryHandler(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<IEnumerable<PurchasesResponse>> Handle(GetUserPurchasesQuery request, CancellationToken cancellationToken)
    {
        var purchases = await _purchaseRepository.GetUserPurchasesAsync(request.UserId);
        return purchases.Select(p => new PurchasesResponse
        {
            Description = p.Description,
            Amount = p.Amount,
            Date = p.Date
        });
    }

    public class CreatePurchaseValidator : AbstractValidator<GetUserPurchasesQuery>
    {
        public CreatePurchaseValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}