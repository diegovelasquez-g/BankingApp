namespace BankingApp.Application.Domain.Entities;

public class CreditCard
{
    public Guid CreditCardId { get; set; }
    public Guid UserId { get; set; }
    public string CardHolder { get; set; } = default!;
    public string CardNumber { get; set; } = default!;
    public decimal CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal BonifiableInterestRate { get; set; }
    public decimal MinimumBalanceInterestRate { get; set; }

}
