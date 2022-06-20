using Application.Repository;
using Infrastructure.Redis.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IIntentionRepository, IntentionRepository>();
        serviceCollection.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        var connectionString = configuration.GetSection("Redis:ConnectionString").Value;

        serviceCollection.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "Identity";
                options.ConfigurationOptions = new ConfigurationOptions
                {
                    EndPoints =
                    {
                        connectionString
                    },
                    ConnectRetry = 5,
                    ReconnectRetryPolicy = new LinearRetry(500),
                    AbortOnConnectFail = false,
                    ConnectTimeout = 5000,
                    SyncTimeout = 5000
                };
            });

        serviceCollection.AddHealthChecks()
            .AddRedis(configuration);

        return serviceCollection;
    }
}