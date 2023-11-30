using Newtonsoft.Json;

namespace Domain.FileSystem;

public class Node
{
    public string Id { get; set; }

    public string Name { get; set; }

    [JsonIgnore]
    public string Type { get; set; }
}
