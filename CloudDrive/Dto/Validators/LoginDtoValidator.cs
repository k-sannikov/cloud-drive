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
                .WithMessage("логин должен включать не менее 3 символов")
                .MaximumLength(255)
                .WithMessage("логин должен включать не более 255 символов")
                .NotEmpty()
                .WithMessage("логин не может быть пустым");

            RuleFor(loginDto => loginDto.Password)
                .NotEmpty()
                .WithMessage("пароль не может быть пустым")
                .MinimumLength(6)
                .WithMessage("пароль должен включать не менее 6 символов")
                .MaximumLength(60)
                .WithMessage("пароль должен включать не более 60 символов");
        }
    }
}
