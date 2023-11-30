using Domain.Auth;

namespace Application.Auth;

public interface IAuthService
{
    Task RegisterUser(User user);
    Task<User> GetUserByCredentionals(string username, string password);
    Task<User> GetUserByToken(string token);
    Task<User> GetUserByUsername(string username);
}
