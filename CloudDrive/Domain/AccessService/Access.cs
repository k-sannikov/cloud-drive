namespace Domain.AccessService;

public class Access
{
    public int UserId { get; set; }
    public string NodeId { get; set; }
    public bool IsOwner { get; set; }

    // Внешние ключи +
    public UserForAccess User { get; set; }
    public NodeForAccess Node { get; set; }
    // Внешние ключи -

    public void SetUser(UserForAccess user)
    {
        User = user;
        UserId = User.Id;
    }

    public void SetNode(NodeForAccess node)
    {
        Node = node;
        NodeId = Node.Id;
    }

    public void SetIsOwner(bool isOwner)
    {
        IsOwner = isOwner;
    }
}
