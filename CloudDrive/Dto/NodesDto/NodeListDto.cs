using Common.ApiUtils;
using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.NodesDto
{
    public class NodeListDto
    {
        [Required]
        [Example("bbc6c975-1f45-4095-9a50-bfe00faf4dd1")]
        public string ParentId { get; set; }

        [Required]
        [Example("Новая папка")]
        public string Name { get; set; }

        [Required]
        public List<NodeDto> Nodes { get; set; }

    }
}
