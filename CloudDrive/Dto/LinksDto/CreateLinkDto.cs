using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.LinksDto;

public class CreateLinkDto
{
    [Required]
    public string ParentId { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    [Required]
    public string Url { get; set; }
}
