namespace BankingApp.Shared.Dtos.Responses;

public class CreditCardResponse
{
    public decimal CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal BonifiableInterestRate { get; set; }
    public decimal MinimumBalanceInterestRate { get; set; }
    public IEnumerable<PurchasesResponse> Purchases { get; set; }
}
