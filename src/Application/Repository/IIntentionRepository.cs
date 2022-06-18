using Application.Models;

namespace Application.Repository;

public interface IIntentionRepository
{
    Task SaveIntentionAsync(Intention intention, DateTimeOffset expireIn, CancellationToken cancellationToken);

    Task<Intention?> GetIntentionAsync(Guid id, CancellationToken cancellationToken);
}