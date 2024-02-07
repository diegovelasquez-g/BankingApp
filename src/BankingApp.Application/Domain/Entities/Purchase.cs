namespace BankingApp.Application.Domain.Entities;

public class Purchase
{
    public Guid PurchaseId { get; set; }
    public Guid CreditCardId { get; set; }
    public string Description { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
