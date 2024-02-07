using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BankingApp.Api.HealthChecks;

public class ServerHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var isServerHealthy = CheckServerStatus();

        return Task.FromResult(isServerHealthy
            ? HealthCheckResult.Healthy("The server it's working.")
            : HealthCheckResult.Unhealthy("The server it's not working."));
    }

    private bool CheckServerStatus()
    {
        return true;
    }
}
