using System.ComponentModel.DataAnnotations;

namespace BankingApp.Shared.Dtos.Requests;

public class NewPurchaseRequest 
{
    public Guid CreditCardId { get; set; }
    [Required(ErrorMessage = "La descripción es requerida")]
    public string Description { get; set; } = default!;
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto tiene que ser mayor a 0")]
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
