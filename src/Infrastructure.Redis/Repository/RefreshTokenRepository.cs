using System.Text.Json;
using Application.Models;
using Application.Repository;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Redis.Repository;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDistributedCache _cache;

    public RefreshTokenRepository(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SaveRefreshTokenAsync(RefreshToken refreshToken, DateTimeOffset expireIn,
        CancellationToken cancellationToken)
    {
        var serialize = JsonSerializer.SerializeToUtf8Bytes(refreshToken, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        await _cache.SetAsync($"RefreshToken:{refreshToken.Token}", serialize, new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = expireIn
        }, cancellationToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string id, CancellationToken cancellationToken)
    {
        var key = $"RefreshToken:{id}";

        var result = await _cache.GetAsync(key, cancellationToken);

        if (result is null || result.Length == 0)
            return null;

        await _cache.RemoveAsync(key, cancellationToken);

        return JsonSerializer.Deserialize<RefreshToken>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}