using Domain.Auth;

namespace Application.Auth;

public interface IAuthService
{
    Task RegisterUser(User user);
    Task<User> GetUser(string username, string password);
}
