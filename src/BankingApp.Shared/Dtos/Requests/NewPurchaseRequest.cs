namespace BankingApp.Shared.Dtos.Requests;

public class NewPurchaseRequest 
{
    public Guid CreditCardId { get; set; }
    public string Description { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
