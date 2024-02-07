using System.Security.Claims;

namespace BankingApp.Client.Helpers;

public class GetAuthUserId
{
    public static Guid GetUserId(ClaimsPrincipal user)
    {
        var userId = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        return Guid.Parse(userId);
    }
}
