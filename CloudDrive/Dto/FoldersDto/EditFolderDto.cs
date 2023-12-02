using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dto.FoldersDto;

public class EditFolderDto
{
    [Required]
    public string Name { get; set; }
}
