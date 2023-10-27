using Domain.Auth;

namespace Domain.AccessSystem;

public class Access
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string  NodeId { get; set; }
    public bool IsOwner  { get; set; }
    
    public User User  { get; set; }
}
