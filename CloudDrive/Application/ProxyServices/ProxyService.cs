using System.Text;

namespace Application.ProxyServices;

public interface IProxyService
{
    HttpResponseMessage ResoveRequest(string key, string id, string body);
}

public class ProxyService : IProxyService
{
    private readonly HttpClient _httpClient;
    private readonly ProxySettings _proxySettings;

    public ProxyService(HttpClient httpClient, ProxySettings proxySettings)
    {
        _httpClient = httpClient;
        _proxySettings = proxySettings;
    }

    public HttpResponseMessage ResoveRequest(string key, string id, string body)
    {
        if (key is null)
        {
            throw new Exception("Key is null");
        }

        foreach (ProxyServiceSettings service in _proxySettings.Services)
        {
            ProxyRoute route = service.Routes.Where(r => r.Key == key).FirstOrDefault();

            if (route is not null)
            {
                Uri fullUrl = new($"{service.ServiceUrl}{route.Path}/{id}");

                HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");

                HttpRequestMessage request = new()
                {
                    RequestUri = fullUrl,
                    Content = content,
                    Method = GetMethod(route.Method),
                };

                request.Headers.Add("X-Api-Key", service.ApiKey);

                HttpResponseMessage response;

                try
                {
                    response = _httpClient.Send(request);
                }
                catch (Exception)
                {
                    throw new Exception("Connection error");
                }

                return response;
            }
        }
        throw new Exception("Request not resolve");
    }

    private static HttpMethod GetMethod(string method)
    {
        return method switch
        {
            "get" => HttpMethod.Get,
            "post" => HttpMethod.Post,
            "put" => HttpMethod.Put,
            "delete" => HttpMethod.Delete,
            _ => null
        };
    }
}
