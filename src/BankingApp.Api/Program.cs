using BankingApp.Api.Filters;
using BankingApp.Api.HealthChecks;
using BankingApp.Application;
using HealthChecks.ApplicationStatus.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>());
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationCore();
builder.Services.AddDomain();
builder.Services.AddPersistance();
builder.Services
    .AddHealthChecks()
    .AddApplicationStatus(name: "api_status", tags: new[] { "api" })
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("sqlConnection")!,
        name: "sql",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "db", "sql", "sqlserver" })
    .AddCheck<ServerHealthCheck>("server_health_check", tags: new[] { "custom", "api" });
builder.Services
    .AddHealthChecksUI()
    .AddInMemoryStorage();
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyHeader())
);

var app = builder.Build();

app.UseCors();
app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
