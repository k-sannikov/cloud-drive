using Application.Auth;
using Domain.Auth;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Auth;

public class UserRepository : IUserRepository
{
    private readonly DbSet<User> _entities;

    public UserRepository(AppDbContext context)
    {
        _entities = context.Set<User>();
    }

    public async Task<User> GetUser(string userId)
    {
        return await _entities.FindAsync(userId);
    }

    public async Task<User> GetByUsername(string username)
    {
        return await _entities
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync();
    }

    public async Task<User> GetByUsernameAndPassword(string username, string password)
    {
        return await _entities
            .Where(u => u.Username == username && u.Password == password)
            .FirstOrDefaultAsync();
    }

    public async Task<User> GetByRefreshToken(string token)
    {
        return await _entities
            .Where(u => u.RefreshToken == token)
            .FirstOrDefaultAsync();
    }

    public async Task AddUser(User user)
    {
        await _entities.AddAsync(user);
    }
}
