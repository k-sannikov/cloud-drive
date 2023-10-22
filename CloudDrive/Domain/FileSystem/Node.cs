
using Domain.AccessService;

namespace Domain.FileSystem;

public class Node
{
    public Guid Id { get; set; }

    public List<Access> Accesses { get; set; }
}
