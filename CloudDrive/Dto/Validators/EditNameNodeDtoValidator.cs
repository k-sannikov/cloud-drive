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
                .WithMessage("название заметки не может быть пустым")
                .MaximumLength(60)
                .WithMessage("название заметки не может включать более 60 символов");
        }
    }
}
