using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.AuthDto;

public class RegisterDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
