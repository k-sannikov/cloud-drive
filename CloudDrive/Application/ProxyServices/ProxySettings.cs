namespace Application.ProxyServices;

public class ProxySettings
{
    public List<ProxyServiceSettings> Services { get; set; }
}
// TODO: вынести в отдельные файлы
public class ProxyServiceSettings
{
    public string ApiKey { get; set; }
    public string ServiceUrl { get; set; }
    public List<ProxyRoute> Routes { get; set; }
}

public class ProxyRoute
{
    public string Key { get; set; }
    public string Method { get; set; }
    public string Path { get; set; }
}