using CloudDrive.Dto.FoldersDto;
using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class EditFolderDtoValidator : AbstractValidator<EditFolderDto>
    {
        public EditFolderDtoValidator()
        {
            RuleFor(editNameDto => editNameDto.Name)
                .NotEmpty()
                .WithMessage("название папки не может быть пустым")
                .MaximumLength(50)
                .WithMessage("название папки не может включать более 50 символов");
        }
    }
}
