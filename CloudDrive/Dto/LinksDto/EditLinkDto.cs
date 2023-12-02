using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.LinksDto;

public class EditLinkDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Url { get; set; }
}
