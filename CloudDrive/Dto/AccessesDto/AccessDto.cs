using Common.ApiUtils;
using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.AccessesDto;

public class AccessDto
{
    [Required]
    [Example(95)]
    public int AccessId { get; set; }

    [Required]
    [Example("ivan.ivanov")]
    public string Username { get; set; }
}
