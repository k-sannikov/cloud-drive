using CloudDrive.Dto.AuthDto;
using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(loginDto => loginDto.Username)
                .MinimumLength(3)
                .MaximumLength(255)
                .NotEmpty();

            RuleFor(loginDto => loginDto.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(60);
        }
    }
}
