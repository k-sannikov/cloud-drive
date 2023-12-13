using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.LinksDto;

public class EditLinkDto
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    [Required]
    public string Url { get; set; }
}
