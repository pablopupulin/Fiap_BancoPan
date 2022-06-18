using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MongoDb.Extensions;

public static class HealthCheckExtensions
{
    public static IHealthChecksBuilder AddMongoDb(this IHealthChecksBuilder builder, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb");
        
        builder.AddMongoDb(connectionString, name: "IdentityMongoDB", tags: new[] {"ready"});

        return builder;
    }
}