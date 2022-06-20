using Application.Models;

namespace Application.Repository;

public interface IRefreshTokenRepository
{
    Task SaveRefreshTokenAsync(RefreshToken refreshToken, DateTimeOffset expireIn, CancellationToken cancellationToken);

    Task<RefreshToken?> GetRefreshTokenAsync(string id, CancellationToken cancellationToken);
}