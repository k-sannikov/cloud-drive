namespace CloudDrive.Auth;

public class AuthSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public int LifeTime { get; set; }
}
