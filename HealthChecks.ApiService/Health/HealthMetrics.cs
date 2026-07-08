using System.Diagnostics.Metrics;

namespace HealthChecks.ApiService.Health;

public class HealthMetrics
{
    private readonly HealthStatusStore _store;

    public HealthMetrics(HealthStatusStore store, IHostEnvironment environment)
    {
        _store = store;

        var meter = new Meter(environment.ApplicationName);

        meter.CreateObservableGauge(
            "service_health",
            () => _store.IsHealthy ? 1 : 0,
            unit: null,
            description: "Overall application health");
    }
}