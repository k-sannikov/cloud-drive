namespace Application.ProxyServices;

public class ProxyServiceSettings
{
    public string UiUrl { get; set; }
    public string Label { get; set; }
    public string Icon { get; set; }
    public string Type { get; set; }
    public string ApiKey { get; set; }
    public string ServiceUrl { get; set; }
    public List<ProxyRoute> Routes { get; set; }
}
