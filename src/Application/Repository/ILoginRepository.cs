using Application.Models;

namespace Application.Repository;

public interface ILoginRepository
{
    Task SaveLoginAsync(Login login, CancellationToken cancellationToken);
}