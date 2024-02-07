using BankingApp.Shared.Dtos.Responses;

namespace BankingApp.Client.Contracts;

public interface ICreditCardsService
{
    Task<List<CreditCardsResponse>> GetCreditCardsAsync(Guid userId);
}
