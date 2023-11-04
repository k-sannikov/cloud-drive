using Domain.AccessSystem;

namespace Domain.Auth;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime  { get; set; }

    public List<Access> Accesses { get; set; }

    public void SetRefreshToken(string token, int expiration)
    {
        RefreshToken = token;
        RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(expiration);
    }
}
