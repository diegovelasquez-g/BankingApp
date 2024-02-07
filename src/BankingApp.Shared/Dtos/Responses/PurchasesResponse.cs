namespace BankingApp.Shared.Dtos.Responses;

public class PurchasesResponse
{
    public string Description { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
