using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.AuthDto;

public class RefreshTokenDto
{
    [Required]
    public string RefreshToken { get; set; }
}
