using Application.CloudDriveIntegrationClient;
using Infrastructure.CloudDriveIntegrationClient.RequestDto;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Infrastructure.CloudDriveIntegrationClient;

public class CloudDriveClient : ICloudDriveClient
{
    private readonly HttpClient _httpClient;
    private readonly CloudDriveApiSettings _settings;

    public CloudDriveClient(HttpClient httpClient, CloudDriveApiSettings settings)
    {
        _httpClient = httpClient;
        _settings = settings;

        _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _settings.ApiKey);
    }

    public async Task<bool> CreateNode(CreateNodeDto createNodeDto)
    {
        Uri fullUrl = new($"{_settings.ServiceUrl}/api/internal/nodes");

        HttpContent content = new StringContent(SerializeInJson(createNodeDto), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteNode(string id)
    {
        Uri fullUrl = new($"{_settings.ServiceUrl}/api/internal/nodes/{id}");

        HttpResponseMessage response = await _httpClient.DeleteAsync(fullUrl);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RenameNode(string id, string name)
    {
        Uri fullUrl = new($"{_settings.ServiceUrl}/api/internal/nodes/{id}");

        EditNodeNameDto editNodeNameDto = new() { Id = id, Name = name };

        HttpContent content = new StringContent(SerializeInJson(editNodeNameDto), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PutAsync(fullUrl, content);

        return response.IsSuccessStatusCode;
    }


    private static string SerializeInJson(object obj)
    {
        DefaultContractResolver contractResolver = new()
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        JsonSerializerSettings jsonSerializerSettings = new()
        {
            ContractResolver = contractResolver,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Formatting = Formatting.None
        };

        return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
    }

}
