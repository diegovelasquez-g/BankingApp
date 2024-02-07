using BankingApp.Shared.Dtos.Requests;
using BankingApp.Shared.Dtos.Responses;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BankingApp.Client.Extensions;

public class AuthenticationExtension : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorageService;
    private ClaimsPrincipal _Notuser = new(new ClaimsIdentity());

    public AuthenticationExtension(ISessionStorageService sessionStorageService)
    {
        _sessionStorageService = sessionStorageService;
    }

    public async Task UpdateAuthState(LoginResponse? loginResponse)
    {
        ClaimsPrincipal claimsPrincipal;
        if (loginResponse is not null)
        {
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim("UserId", loginResponse.UserId.ToString()),
                new Claim(ClaimTypes.Name, loginResponse.Username),
                new Claim(ClaimTypes.Email, loginResponse.Email)

            }, "apiauth"));

            await _sessionStorageService.SaveSession("userSession", loginResponse);
        }
        else
        {
            claimsPrincipal = _Notuser;
            await _sessionStorageService.RemoveItemAsync("userSession");
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var userSession = await _sessionStorageService.GetSession<LoginResponse>("userSession");

        if (userSession == null)
            return await Task.FromResult(new AuthenticationState(_Notuser));

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim("UserId", userSession. UserId.ToString()),
            new Claim(ClaimTypes.Name, userSession.Username),
            new Claim(ClaimTypes.Email, userSession.Email)
        }, "apiauth"));

        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }
}
