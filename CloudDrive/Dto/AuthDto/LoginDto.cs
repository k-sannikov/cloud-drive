using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.AuthDto;

public class LoginDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
