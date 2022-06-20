using Application.Repository;
using Infrastructure.MongoDb.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Infrastructure.MongoDb.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<ILoginRepository, LoginRepository>();

        var connectionString = configuration.GetSection("MongoDb:ConnectionString").Value;

        serviceCollection.AddScoped<IMongoClient, MongoClient>(_ =>
            new MongoClient(connectionString));

        serviceCollection.AddHealthChecks()
            .AddMongoDb(configuration);

        AddConventions();

        return serviceCollection;
    }

    private static void AddConventions()
    {
        var pack = new ConventionPack {new IgnoreExtraElementsConvention(true)};
        ConventionRegistry.Register("Conventions", pack, t => true);
    }
}