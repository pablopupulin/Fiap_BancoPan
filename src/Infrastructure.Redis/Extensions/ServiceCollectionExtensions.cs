using Application.Repository;
using Infrastructure.Redis.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IIntentionRepository, IntentionRepository>();

        serviceCollection.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "Identity";
            });

        serviceCollection.AddHealthChecks()
            .AddRedis(configuration);

        return serviceCollection;
    }
}