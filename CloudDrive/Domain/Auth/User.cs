using Domain.AccessSystem;

namespace Domain.Auth;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public List<Access> Accesses { get; set; }
}
