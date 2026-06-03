using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OrbitPass.Api.Extensions;

public static class HealthCheckExtensions {
    public static IServiceCollection AddHealthChecksConfig(
        this IServiceCollection services, string connectionString) {
        services.AddHealthChecks()
            .AddOracle(
                connectionString: connectionString,
                name: "oracle-db",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "oracle" })
            .AddCheck("api-self",
                () => HealthCheckResult.Healthy("API online"),
                tags: new[] { "api" });

        return services;
    }
}