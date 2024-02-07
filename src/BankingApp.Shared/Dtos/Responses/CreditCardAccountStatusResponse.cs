namespace BankingApp.Shared.Dtos.Responses;

public class CreditCardAccountStatusResponse
{
    public Guid IdTarjeta { get; set; }
    public string TitularTarjeta { get; set; }
    public string NumeroTarjeta { get; set; }
    public decimal SaldoActual { get; set; }
    public decimal LimiteCredito { get; set; }
    public decimal SaldoDisponible { get; set; }
    public decimal TotalComprasMesActual { get; set; }
    public decimal TotalComprasMesAnterior { get; set; }
    public decimal InteresBonificable { get; set; }
    public decimal CuotaMinimaAPagar { get; set; }
    public decimal MontoTotalContadoIntereses { get; set; }
    public decimal MontoTotalSinIntereses { get; set; }
    public IEnumerable<PurchasesResponse>? Purchases { get; set;}
}
