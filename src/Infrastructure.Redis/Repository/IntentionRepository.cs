using System.Text.Json;
using Application.Models;
using Application.Repository;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Redis.Repository;

public class IntentionRepository : IIntentionRepository
{
    private readonly IDistributedCache _cache;

    public IntentionRepository(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SaveIntentionAsync(Intention intention, DateTimeOffset expireIn,
        CancellationToken cancellationToken)
    {
        var serialize = JsonSerializer.SerializeToUtf8Bytes(intention);

        await _cache.SetAsync($"Intention:{intention.IntentionId}", serialize, new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = expireIn
        }, cancellationToken);
    }

    public async Task<Intention?> GetIntentionAsync(Guid id, CancellationToken cancellationToken)
    {
        var key = $"Intention:{id}";
        var result = await _cache.GetAsync(key, cancellationToken);

        if (result is null || result.Length == 0) 
            return null;

        await _cache.RemoveAsync(key, cancellationToken);

        return JsonSerializer.Deserialize<Intention>(result);
    }
}