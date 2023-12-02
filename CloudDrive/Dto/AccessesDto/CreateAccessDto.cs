using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.AccessesDto;

public class CreateAccessDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string NodeId { get; set; }
}
