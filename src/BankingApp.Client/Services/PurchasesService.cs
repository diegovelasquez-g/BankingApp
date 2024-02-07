using BankingApp.Client.Contracts;
using BankingApp.Shared.Dtos.Requests;
using BankingApp.Shared.Dtos.Responses;
using System.Net.Http.Json;

namespace BankingApp.Client.Services;

public class PurchasesService : IPurchasesService
{
    private readonly HttpClient _httpClient;

    public PurchasesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PurchasesResponse>> GetPurchasesAsync(Guid userId)
    {
        var response = await _httpClient.GetFromJsonAsync<List<PurchasesResponse>>($"api/Purchases/myPurchases?userId={userId}");
        return response ?? new List<PurchasesResponse>();
    }

    public async Task<bool> NewPurchaseAsync(NewPurchaseRequest newPurchaseRequest)
    {
        var response = await _httpClient.PostAsJsonAsync<NewPurchaseRequest>("api/Purchases/newPurchase", newPurchaseRequest);

        if(response.IsSuccessStatusCode)
            return true;
  
        return false;
    }
}
