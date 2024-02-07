namespace BankingApp.Shared.Dtos.Responses;

public class LoginResponse
{
    public Guid UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
}
