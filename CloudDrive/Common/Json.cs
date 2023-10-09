using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Common;

public static class Json
{
    public static T Deserialize<T>(string json)
    {
        DefaultContractResolver contractResolver = new()
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        JsonSerializerSettings jsonSerializerSettings = new()
        {
            ContractResolver = contractResolver,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Formatting = Newtonsoft.Json.Formatting.None
        };

        return JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);
    }

    public static string Serialize(object obj)
    {
        DefaultContractResolver contractResolver = new()
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };

        JsonSerializerSettings jsonSerializerSettings = new()
        {
            ContractResolver = contractResolver,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Formatting = Newtonsoft.Json.Formatting.None,
        };

        return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
    }
}
