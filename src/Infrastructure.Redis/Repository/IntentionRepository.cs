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

    public async Task SaveIntentionAsync(Intention intention, DateTimeOffset expireIn, CancellationToken cancellationToken)
    {
        var serialize = JsonSerializer.SerializeToUtf8Bytes(intention);

        await _cache.SetAsync(intention.IntentionId.ToString(), serialize, new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = expireIn
        }, cancellationToken);
    }

    public async Task<Intention?> GetIntentionAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cache.GetAsync(id.ToString(), cancellationToken);
        
        return result is null || result.Length == 0 ? 
            null : 
            JsonSerializer.Deserialize<Intention>(result);
    }
}