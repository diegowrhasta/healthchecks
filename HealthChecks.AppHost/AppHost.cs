var builder = DistributedApplication.CreateBuilder(args);

// Prometheus
var prometheus = builder.AddContainer("prometheus", "prom/prometheus:main-distroless")
    .WithBindMount("./prometheus/prometheus.yml", "/etc/prometheus/prometheus.yml")
    .WithHttpEndpoint(targetPort: 9090, port: 9090)
    .WithLifetime(ContainerLifetime.Persistent);

// Grafana
var grafana = builder.AddContainer("grafana", "grafana/grafana:nightly-ubuntu-slim")
    .WithEnvironment("GF_SECURITY_ADMIN_USER", "admin")
    .WithEnvironment("GF_SECURITY_ADMIN_PASSWORD", "admin")
    .WithBindMount(
        "./grafana/provisioning",
        "/etc/grafana/provisioning")
    .WithBindMount(
        "./grafana/dashboards",
        "/var/lib/grafana/dashboards")
    .WithHttpEndpoint(targetPort: 3000, port: 3000)
    .WithLifetime(ContainerLifetime.Persistent)
    .WaitFor(prometheus);

// var alloy = builder.AddContainer("alloy", "grafana/alloy:v1.17.1")
//     .WithBindMount(
//         "./alloy/config.alloy",
//         "/etc/alloy/config.alloy")
//     .WithHttpEndpoint(
//         targetPort: 12345,
//         port: 12345,
//         name: "ui")
//     .WithEndpoint(
//         targetPort: 4317,
//         port: 4317,
//         name: "otlp-grpc")
//     .WithEndpoint(
//         targetPort: 4318,
//         port: 4318,
//         name: "otlp-http")
//     .WithEndpoint(
//         targetPort: 9464,
//         port: 9464,
//         name: "prometheus")
//     .WithArgs(
//         "run",
//         "/etc/alloy/config.alloy")
//     .WithLifetime(ContainerLifetime.Persistent)
//     .WaitFor(prometheus);

var apiService = builder
    .AddDockerfile(name: "apiservice", contextPath: "../", dockerfilePath: "./HealthChecks.ApiService/Dockerfile")
    .WithHttpEndpoint(targetPort: 8080, port: 5591)
    .WithHttpHealthCheck("/health");
    // .WithEnvironment("OTEL_EXPORTER_OTLP_ENDPOINT", "http://alloy:4317")
    // .WithEnvironment("OTEL_EXPORTER_OTLP_PROTOCOL", "grpc");

builder
    .AddDockerfile(name: "webfrontend", contextPath: "../", dockerfilePath: "./HealthChecks.Web/Dockerfile")
    .WithHttpEndpoint(targetPort: 8080, port: 5208)
    .WithHttpHealthCheck("/health")
    .WaitFor(apiService);

builder.Build().Run();