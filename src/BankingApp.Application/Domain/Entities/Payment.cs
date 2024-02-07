namespace BankingApp.Application.Domain.Entities;

public class Payment
{
    public Guid PaymentId { get; set; }
    public Guid CreditCardId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
