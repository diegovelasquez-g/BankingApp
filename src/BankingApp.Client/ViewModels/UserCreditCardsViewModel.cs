namespace BankingApp.Client.ViewModels;

public class UserCreditCardsViewModel
{
    public Guid CreditCardId { get; set; }
    public string CardNumber { get; set; } = default!; 
    public decimal AvailableBalance { get; set; }
}
