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

    public async Task<User> GetUser(int userId)
    {
        return await _entities.FindAsync(userId);
    }
}
