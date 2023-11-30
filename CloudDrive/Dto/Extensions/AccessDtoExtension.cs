using CloudDrive.Dto.AccessesDto;
using Domain.AccessSystem;

namespace CloudDrive.Dto.Extensions;

public static class AccessDtoExtension
{
    public static Access ToDomain(this CreateAccessDto createAccessDto, string userId)
    {
        return new()
        {
            UserId = userId,
            NodeId = createAccessDto.NodeId,
            IsOwner = false,
            IsRoot = false,
        };
    }

    public static AccessDto ToDto(this Access access)
    {
        return new()
        {
            AccessId = access.Id,
            Username = access.User.Username,
        };
    }
}
