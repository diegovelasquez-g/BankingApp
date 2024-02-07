using BankingApp.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using BankingApp.Client.Extensions;
using CurrieTechnologies.Razor.SweetAlert2;
using BankingApp.Client.Contracts;
using BankingApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7105") });

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<AuthenticationStateProvider,AuthenticationExtension>();
builder.Services.AddAuthorizationCore();
builder.Services.AddSweetAlert2();
builder.Services.AddScoped<IPurchasesService, PurchasesService>();
builder.Services.AddScoped<ICreditCardsService, CreditCardsService>();
builder.Services.AddScoped<IPaymentsService, PaymentsService>();

await builder.Build().RunAsync();
