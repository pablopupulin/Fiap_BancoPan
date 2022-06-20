using Application.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Infrastructure.MongoDb.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider Seed(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var client = scope.ServiceProvider.GetRequiredService<IMongoClient>();

        var userCollection = client.GetDatabase("Identity").GetCollection<User>(nameof(User));

        userCollection.DeleteMany(c => true);

        userCollection.InsertMany(new[]
        {
            new User
            {
                Name = "User1",
                Email = "User1@email.com",
                Document = "12345678912",
                UserId = Guid.NewGuid()
            }.ChangePassword("123456"),

            new User
            {
                Name = "User2",
                Email = "User2@email.com",
                Document = "98765432109",
                UserId = Guid.NewGuid()
            }.ChangePassword("654321")
        });

        return services;
    }
}