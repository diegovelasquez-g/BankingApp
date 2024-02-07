namespace BankingApp.Shared.Dtos.Responses;

public class TransactionsResponse
{
    public string TransactionType { get; set; } = default!;
    public DateTime TransactionDate { get; set; }
    public decimal TransactionAmount { get; set; }
}
