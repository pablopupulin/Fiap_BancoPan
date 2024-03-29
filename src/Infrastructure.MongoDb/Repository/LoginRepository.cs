﻿using Application.Models;
using Application.Repository;
using MongoDB.Driver;

namespace Infrastructure.MongoDb.Repository;

public class LoginRepository: ILoginRepository
{
    private readonly IMongoCollection<Login> _collection;

    public LoginRepository(IMongoClient mongoClient)
    {
        var identity = mongoClient.GetDatabase("Identity");
        _collection = identity.GetCollection<Login>(nameof(Login));
    }

    public async Task SaveLoginAsync(Login login, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(login, cancellationToken: cancellationToken);
    }
}