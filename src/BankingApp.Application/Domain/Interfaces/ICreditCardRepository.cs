using BankingApp.Application.Domain.Entities;

namespace BankingApp.Application.Domain.Interfaces;

public interface ICreditCardRepository
{
    public Task<IEnumerable<CreditCard>> GetCreditCardsByUserIdAsync(Guid userId);
}
