using Application.AccessApp.Repository;
using Domain.AccessService;
using Domain.FileSystem;
using Domain.UserService;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AccessService.Repository;

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

    public void DeleteAccess(Access access)
    {
        _entities.Remove(access);
    }

    public async Task<List<Access>> GetAccesses(User user, List<Node> nodes)
    {
        List<Guid> nodesGuids = new List<Guid>();
        
        foreach (Node node in nodes)
        {
            nodesGuids.Add(node.Id);
        }

        return await _entities.Where(a => a.UserId == user.Id && nodesGuids.Contains(a.NodeId)).ToListAsync();
    }

    public async Task<List<Access>> GetAllByUser(User user)
    {
        return await _entities.Where(a => a.UserId == user.Id).ToListAsync();
    }

    public async Task<List<Access>> GetAllByNode(Node node)
    {
        return await _entities.Where(a => a.NodeId == node.Id).ToListAsync();
    }

    public async Task<Access> GetAccess(User user, Node node)
    {
        return await _entities.FirstOrDefaultAsync(a => a.UserId == user.Id && a.NodeId == node.Id);
    }
}
