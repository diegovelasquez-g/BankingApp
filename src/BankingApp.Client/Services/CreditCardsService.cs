using BankingApp.Client.Contracts;
using BankingApp.Shared.Dtos.Responses;
using System.Net.Http.Json;

namespace BankingApp.Client.Services;

public class CreditCardsService : ICreditCardsService
{
    private readonly HttpClient _httpClient;

    public CreditCardsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CreditCardsResponse>> GetCreditCardsAsync(Guid userId)
    {
        var response = await _httpClient.GetFromJsonAsync<CreditCardsResponse[]>($"api/CreditCards/myCreditCards?userId={userId}");
        return response.ToList();
    }
}
