namespace BankingApp.Shared.Dtos.Responses;

public class CreditCardsResponse
{
    public Guid CreditCardId { get; set; }
    public string CardHolderName { get; set; } = default!;
    public string CardNumber { get; set; } = default!;
    public decimal AvailableBalance { get; set; }
}
