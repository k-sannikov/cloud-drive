using CloudDrive.Dto.LinksDto;
using Domain.FileSystem;

namespace CloudDrive.Dto.Extensions;

public static class LinkDtoExtension
{
    public static LinkDto ToDto(this Link node)
    {
        return new LinkDto()
        {
            Id = node.Id,
            Name = node.Name,
            Type = node.Type.ToString(),
            Url = node.Url,
            Description = node.Description,
        };
    }
}
