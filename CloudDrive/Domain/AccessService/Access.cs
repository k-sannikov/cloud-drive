using Domain.FileSystem;
using Domain.UserService;

namespace Domain.AccessService;

public class Access
{
    public int UserId { get; set; }
    public Guid NodeId { get; set; }
    public bool IsOwner { get; set; }

    // Внешние ключи +
    public User User { get; set; }
    public Node Node { get; set; }
    // Внешние ключи -

    public void SetUser(User user)
    {
        User = user;
        UserId = User.Id;
    }

    public void SetNode(Node node)
    {
        Node = node;
        NodeId = Node.Id;
    }

    public void SetIsOwner()
    {
        IsOwner = true;
    }

    public void SetIsNotOwner()
    {
        IsOwner = false;
    }
}
