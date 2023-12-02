using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.FoldersDto;

public class CreateFolderDto
{
    [Required]
    public string ParentId { get; set; }
    [Required]
    public string Name { get; set; }
}
