using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.NodesDto;

public class CreateNodeDto
{
    [Required]
    public string ParentId { get; set; }
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Type { get; set; }
}
