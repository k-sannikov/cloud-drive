using Domain.Auth;

namespace Application.Auth;

public interface IUserRepository
{
    Task<User> GetUser(string userId);
    Task<User> GetByUsername(string username);
    Task<User> GetByUsernameAndPassword(string username, string password);
    Task AddUser(User user);
}
