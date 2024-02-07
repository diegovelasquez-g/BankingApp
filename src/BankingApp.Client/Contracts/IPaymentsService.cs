using BankingApp.Shared.Dtos.Requests;
using BankingApp.Shared.Dtos.Responses;

namespace BankingApp.Client.Contracts;

public interface IPaymentsService
{
    Task<List<PaymentsResponse>> GetPaymentsAsync(Guid userId);
    Task<bool> NewPaymentAsync(NewPaymentRequest newPaymentRequest);
}
