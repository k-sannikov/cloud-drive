using Application.FileSystem;
using Domain.FileSystem;
using Neo4j.Driver;
using Neo4j.Driver.Preview.Mapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.FileSystem;

public class FileSystemRepository : IFileSystemRepository
{
    private readonly IDriver _driver;

    private const string _relation = "CHILD";

    public FileSystemRepository(IDriver driver)
    {
        _driver = driver;
    }
    public async Task AddNode(Node node)
    {
        string body = Serialize(node);

        string query = $"MERGE (:{node.Type} {body})";

        EagerResult<IReadOnlyList<IRecord>> response = await _driver.ExecutableQuery(query)
            .ExecuteAsync();

        ICounters counters = response.Summary.Counters;

        if (counters.NodesCreated == 0)
        {
            throw new Exception("Node not created");
        }
    }

    public async Task<IReadOnlyList<T>> GetNodes<T>(IEnumerable<string> nodeIds) where T : Node, new()
    {
        string body = Serialize(nodeIds);

        string query = $"MATCH (node) WHERE node.id IN {body} RETURN node";

        EagerResult<IReadOnlyList<IRecord>> response = await _driver.ExecutableQuery(query)
            .ExecuteAsync();

        return response.Result.Select(r => ToNode<T>(r)).ToList();
    }

    public async Task RenameNode(string nodeId, string newName)
    {
        string query =
            $"MATCH (node)" +
            $"WHERE node.id = \"{nodeId}\"" +
            $"SET node.name = \"{newName}\"";

        EagerResult<IReadOnlyList<IRecord>> response = await _driver.ExecutableQuery(query)
            .ExecuteAsync();

        ICounters counters = response.Summary.Counters;

        if (counters.PropertiesSet == 0)
        {
            throw new Exception("Node not rename");
        }
    }

    public async Task EditNode<T>(Node node) where T : Node, new()
    {
        string body = Serialize((T)node);

        string query = $"MATCH (node:{node.Type})" +
            $"WHERE node.id = \"{node.Id}\"" +
            $"SET node = {body}";

        EagerResult<IReadOnlyList<IRecord>> response = await _driver.ExecutableQuery(query)
            .ExecuteAsync();

        ICounters counters = response.Summary.Counters;

        if (counters.PropertiesSet == 0)
        {
            throw new Exception("Node not edit");
        }
    }

    public async Task AddNodeWithRelation(Node node, string parentId)
    {
        string body = Serialize(node);

        string query =
            $"MATCH (parent:{NodeType.Folder}) WHERE parent.id = \"{parentId}\"" +
            $"MERGE (child:{node.Type} {body})" +
            $"CREATE (child)-[:{_relation}]->(parent)";

        EagerResult<IReadOnlyList<IRecord>> response = await _driver.ExecutableQuery(query)
            .ExecuteAsync();

        ICounters counters = response.Summary.Counters;

        if (counters.NodesCreated == 0 || counters.RelationshipsCreated == 0)
        {
            throw new Exception("Node or relationships not created");
        }
    }

    public async Task DeleteNodeWithChildren(string nodeId)
    {
        string queryMultiDelete =
            $"MATCH (parent)<-[:{_relation}*]-(child)" +
            $"WHERE parent.id = \"{nodeId}\"" +
            $"DETACH DELETE child, parent";

        EagerResult<IReadOnlyList<IRecord>> responseMultiDelete = await _driver.ExecutableQuery(queryMultiDelete)
            .ExecuteAsync();

        string querySingleDelete = $"MATCH (node) WHERE node.id = \"{nodeId}\" DELETE node";

        EagerResult<IReadOnlyList<IRecord>> responseSingleDelete = await _driver.ExecutableQuery(querySingleDelete)
            .ExecuteAsync();

        ICounters countersMultiDelete = responseMultiDelete.Summary.Counters;
        ICounters countersSingleDelete = responseSingleDelete.Summary.Counters;

        if (countersMultiDelete.NodesDeleted == 0 && countersSingleDelete.NodesDeleted == 0)
        {
            throw new Exception("Node not deleted");
        }
    }

    public async Task<IReadOnlyList<Node>> GetParentsNodes(string nodeId)
    {
        string query =
            $"MATCH (parent:{NodeType.Folder})<-[:{_relation}*]-(child)" +
            $"WHERE child.id = \"{nodeId}\"" +
            $"RETURN parent";

        EagerResult<IReadOnlyList<IRecord>> response = await _driver.ExecutableQuery(query)
            .ExecuteAsync();

        return response.Result.Select(r => ToNode<Node>(r)).ToList();
    }

    public async Task<IReadOnlyList<Node>> GetChildsNodes(string nodeId)
    {
        string query =
            $"MATCH (parent)-[:{_relation}]->(child)" +
            $"WHERE child.id = \"{nodeId}\"" +
            $"RETURN parent";

        EagerResult<IReadOnlyList<IRecord>> response = await _driver.ExecutableQuery(query)
            .ExecuteAsync();

        return response.Result.Select(r => ToNode<Node>(r)).ToList();
    }

    private static string Serialize(object obj)
    {
        DefaultContractResolver contractResolver = new()
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };

        JsonSerializerSettings jsonSerializerSettings = new()
        {
            ContractResolver = contractResolver,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Formatting = Formatting.None,
        };

        JsonSerializer serializer = JsonSerializer.Create(jsonSerializerSettings);

        StringWriter stringWriter = new();
        using (JsonTextWriter writer = new(stringWriter))
        {
            writer.QuoteName = false;
            serializer.Serialize(writer, obj);
        }

        return stringWriter.ToString();
    }

    private static T ToNode<T>(IRecord record) where T: Node, new()
    {
        T node = record.AsObject<T>();
        INode rawNode = (INode)record.Values.First().Value;

        string type = rawNode.Labels[0];
        node.Type = (NodeType)Enum.Parse(typeof(NodeType), type);

        return node;
    }
}

