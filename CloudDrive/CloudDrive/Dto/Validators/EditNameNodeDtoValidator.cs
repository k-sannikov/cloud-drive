using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class EditNameNodeDtoValidator : AbstractValidator<EditNameNodeDto>
    {
        public EditNameNodeDtoValidator()
        {
            RuleFor(createFolderDto => createFolderDto.Name)
                .NotEmpty()
                .MaximumLength(60);
        }
    }
}
