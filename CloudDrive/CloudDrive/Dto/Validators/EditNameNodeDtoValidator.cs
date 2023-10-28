using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class EditNameNodeDtoValidator : AbstractValidator<EditNameNodeDto>
    {
        public EditNameNodeDtoValidator()
        {
            RuleFor(editNameDto => editNameDto.Name)
                .NotEmpty()
                .MaximumLength(60);
        }
    }
}
