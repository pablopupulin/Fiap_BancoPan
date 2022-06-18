using Application.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Infrastructure.MongoDb.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication Seed(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();

        var client = scope.ServiceProvider.GetRequiredService<IMongoClient>();

        var userCollection = client.GetDatabase("Identity").GetCollection<User>(nameof(User));

        if (userCollection.CountDocuments(_ => true) == 0)
            userCollection.InsertMany(new[]
            {
                new User
                {
                    Document = "12345678912",
                    UserId = Guid.NewGuid()
                }.ChangePassword("123"),

                new User
                {
                    Document = "98765432109",
                    UserId = Guid.NewGuid()
                }.ChangePassword("321")
            });

        return application;
    }
}