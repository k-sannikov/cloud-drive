using Common.ApiUtils;
using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.ProxyDto;

public class ProxyServiceDto
{
    [Required]
    [Example("https://example.com")]
    public string UiUrl { get; set; }

    [Required]
    [Example("Создать заметку")]
    public string Label { get; set; }

    [Required]
    [Example("PHN2ZyB3aWR0aD0iMjUiIGhlaWdodD0iMjYiIHZpZXdCb3g9IjAgMCAyNS")]
    public string Icon { get; set; }

}
