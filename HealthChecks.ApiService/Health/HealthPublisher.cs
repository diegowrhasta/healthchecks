using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.ApiService.Health;

public class HealthPublisher(HealthStatusStore store) : IHealthCheckPublisher
{
    public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
    {
        store.IsHealthy = report.Status == HealthStatus.Healthy;

        return Task.CompletedTask;
    }
}