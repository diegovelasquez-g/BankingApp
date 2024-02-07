using BankingApp.Shared.Dtos.Requests;
using BankingApp.Shared.Dtos.Responses;

namespace BankingApp.Client.Contracts;

public interface IPurchasesService
{
    Task<List<PurchasesResponse>> GetPurchasesAsync(Guid userId);
    Task<bool> NewPurchaseAsync(NewPurchaseRequest newPurchaseRequest);

}