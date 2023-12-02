using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.NodesDto;

public class RenameNodeDto
{
    [Required]
    public string Name { get; set; }
}
