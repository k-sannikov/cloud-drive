namespace CloudDrive.Dto
{
    public class NodeListDto
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
        public List<NodeDto> Nodes { get; set; }

    }
}
