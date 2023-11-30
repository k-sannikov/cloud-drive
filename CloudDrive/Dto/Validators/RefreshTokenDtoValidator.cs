using CloudDrive.Dto.AuthDto;
using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenDtoValidator()
        {
            RuleFor(refreshTokenDto => refreshTokenDto.RefreshToken)
                .NotEmpty();
        }
    }
}
