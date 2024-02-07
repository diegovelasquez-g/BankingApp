namespace BankingApp.Shared.Dtos.Requests;

public class NewPaymentRequest
{
    public Guid CreditCardId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
