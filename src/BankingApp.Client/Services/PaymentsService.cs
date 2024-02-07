using BankingApp.Client.Contracts;
using BankingApp.Client.Pages;
using BankingApp.Shared.Dtos.Requests;
using BankingApp.Shared.Dtos.Responses;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace BankingApp.Client.Services;

public class PaymentsService : IPaymentsService
{
    private readonly HttpClient _httpClient;

    public PaymentsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PaymentsResponse>> GetPaymentsAsync(Guid userId)
    {

        var response = await _httpClient.GetFromJsonAsync<List<PaymentsResponse>>($"api/Payments/myPayments?userId={userId}");
        return response ?? new List<PaymentsResponse>();

    }

    public async Task<bool> NewPaymentAsync(NewPaymentRequest newPaymentRequest)
    {
        var response = await _httpClient.PostAsJsonAsync<NewPaymentRequest>("api/Payments/newPayment", newPaymentRequest);
        if(response.IsSuccessStatusCode)
            return true;

        return false;
    }
}
