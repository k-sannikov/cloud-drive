﻿using CloudDrive.Dto.AuthDto;
using Domain.Auth;

namespace CloudDrive.Dto.Extensions;

public static class UserDtoExtension
{
    public static User ToDomain(this RegisterDto user)
    {
        return new User()
        {
            Username = user.Username,
            Password = user.Password,
        };
    }
}
