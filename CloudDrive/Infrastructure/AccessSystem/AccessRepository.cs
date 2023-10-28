using Application.AccessSystem;
using Domain.AccessSystem;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AccessSystem;

public class AccessRepository : IAccessRepository
{
    private readonly DbSet<Access> _entities;

    public AccessRepository(AppDbContext context)
    {
        _entities = context.Set<Access>();
    }

    public async Task AddAccess(Access access)
    {
        await _entities.AddAsync(access);
    }

    public async Task<Access> GetAccess(int accessId)
    {
        return await _entities.FindAsync(accessId);
    }

    public async Task<Access> GetByUserIdAndNodeId(string userId, string nodeId)
    {
        return await _entities
            .Where(a => a.UserId == userId && a.NodeId == nodeId)
            .FirstOrDefaultAsync();
    }

    public async Task<Access> GetRootByUserId(string userId)
    {
        return await _entities
            .Where(a => a.UserId == userId && a.IsRoot)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Access>> GetByNodeIds(List<string> nodeIds)
    {
        return await _entities
            .Where(a => nodeIds.Contains(a.NodeId))
            .ToListAsync();
    }

    public void DeleteAccess(Access access)
    {
        _entities.Remove(access);
    }
}
