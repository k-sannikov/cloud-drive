using Common.ApiUtils;
using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.LinksDto;

public class LinkDto
{
    [Required]
    [Example("96feace4-7d76-4f74-ba4e-8d31e843a46f")]
    public string Id { get; set; }

    [Required]
    [Example("Ссылка на VK")]
    public string Name { get; set; }

    [Required]
    [Example("Очень нужная ссылка")]
    public string Description { get; set; }

    [Required]
    [Example("https://vk.com")]
    public string Url { get; set; }

    [Required]
    [Example("Link")]
    public string Type { get; set; }
}
