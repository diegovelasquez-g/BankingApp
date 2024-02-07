using Blazored.SessionStorage;
using System.Text.Json;

namespace BankingApp.Client.Extensions;

public static class SessionStorageExtension
{
    public static async Task SaveSession<T>(this ISessionStorageService sessionStorageService, string key, T value) where T : class
    {
        var jsonItem = JsonSerializer.Serialize(value);
        await sessionStorageService.SetItemAsStringAsync(key, jsonItem);
    }

    public static async Task<T?> GetSession<T>(this ISessionStorageService sessionStorageService, string key) where T : class
    {
        var jsonItem = await sessionStorageService.GetItemAsStringAsync(key);
        if(jsonItem is not null)
        {
            var item = JsonSerializer.Deserialize<T>(jsonItem);
            return item;
        }
        else
        {
            return null;
        }

    }
}
