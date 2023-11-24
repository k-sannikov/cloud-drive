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
                .MaximumLength(255)
                .NotEmpty();

            RuleFor(item => item)
                .MustAsync(async (item, cancellation) => await _authValidationRules.IsUniqueUsername(item.Username))
                .WithMessage(item => "Username is not unique");

            RuleFor(registerDto => registerDto.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(60);
        }
    }
}
