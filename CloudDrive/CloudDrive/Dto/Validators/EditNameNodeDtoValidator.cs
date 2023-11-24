using CloudDrive.Dto.FoldersDto;
using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class EditNameNodeDtoValidator : AbstractValidator<EditFolderDto>
    {
        public EditNameNodeDtoValidator()
        {
            RuleFor(editNameDto => editNameDto.Name)
                .NotEmpty()
                .MaximumLength(60);
        }
    }
}
