using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Redis.Extensions;

public static class HealthCheckExtensions
{
    public static IHealthChecksBuilder AddRedis(this IHealthChecksBuilder builder, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis");

        builder.AddRedis(connectionString, "IdentityRedis", tags: new[] {"ready"});

        return builder;
    }
}