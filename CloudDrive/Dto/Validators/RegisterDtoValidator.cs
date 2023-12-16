using Application.Auth;
using Application.Validation;
using CloudDrive.Dto.AuthDto;
using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        private readonly AuthValidationRules _authValidationRules;

        public RegisterDtoValidator(IUserRepository userRepository)
        {
            _authValidationRules = new AuthValidationRules(userRepository);

            RuleFor(registerDto => registerDto.Username)
                .MinimumLength(3)
                .WithMessage("имя пользователя должно включать не менее 3 символов")
                .MaximumLength(25)
                .WithMessage("имя пользователя должно включать не более 25 символов")
                .NotEmpty()
                .WithMessage("имя пользователя не может быть пустым");

            RuleFor(item => item)
                .MustAsync(async (item, cancellation) => await _authValidationRules.IsUniqueUsername(item.Username))
                .WithMessage(item => "имя пользователя должно быть уникально");

            RuleFor(registerDto => registerDto.Password)
                .NotEmpty()
                .WithMessage("пароль не может быть пустым")
                .MinimumLength(6)
                .WithMessage("пароль должен включать не менее 6 символов")
                .MaximumLength(25)
                .WithMessage("пароль должен включать не более 25 символов");
        }
    }
}
