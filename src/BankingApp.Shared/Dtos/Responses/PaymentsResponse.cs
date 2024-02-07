namespace BankingApp.Shared.Dtos.Responses;

public class PaymentsResponse
{
    public Guid CreditCardId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
}
