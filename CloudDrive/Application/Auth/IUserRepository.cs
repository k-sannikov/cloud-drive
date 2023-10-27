using Domain.Auth;

namespace Application.Auth;

public interface IUserRepository
{
    Task<User> GetUser(int userId);
}
