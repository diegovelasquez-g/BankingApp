using System.ComponentModel.DataAnnotations;

namespace BankingApp.Shared.Dtos.Requests;

public class NewPaymentRequest
{
    public Guid CreditCardId { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto tiene que ser mayor a 0")]
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
