using BankingApp.Application.Domain.Entities;

namespace BankingApp.Application.Domain.Interfaces;

public interface IPurchaseRepository
{
    Task<bool> AddAsync(Purchase purchase);
    Task<IEnumerable<Purchase>> GetUserPurchasesAsync(Guid userId);
    Task<IEnumerable<Purchase>> GetCreditCardPurchasesAsync(Guid creditCardId);
}
