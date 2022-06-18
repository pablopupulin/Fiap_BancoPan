using Application.Models;
using Application.Repository;
using MongoDB.Driver;

namespace Infrastructure.MongoDb.Repository;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _collection;

    public UserRepository(IMongoClient mongoClient)
    {
        var identity = mongoClient.GetDatabase("Identity");
        _collection = identity.GetCollection<User>(nameof(User));
    }

    public async Task<User?> GetUserAsync(string document)
    {
        var cursor = await _collection.FindAsync(c => c.Document == document);

        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserAsync(Guid userId)
    {
        var cursor = await _collection.FindAsync(c => c.UserId == userId);

        return await cursor.FirstOrDefaultAsync();
    }
}