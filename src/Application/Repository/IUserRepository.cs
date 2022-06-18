using Application.Models;

namespace Application.Repository;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string intentionUser);
    Task<User?> GetUserAsync(Guid userId);
}