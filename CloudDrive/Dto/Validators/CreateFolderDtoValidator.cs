using CloudDrive.Dto.FoldersDto;
using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class CreateFolderDtoValidator : AbstractValidator<CreateFolderDto>
    {
        public CreateFolderDtoValidator()
        {
            RuleFor(createFolderDto => createFolderDto.ParentId)
                .NotEmpty()
                .WithMessage("id родительской папки не может быть пустым");

            RuleFor(createFolderDto => createFolderDto.Name)
                .NotEmpty()
                .WithMessage("название папки не может быть пустым")
                .MaximumLength(255)
                .WithMessage("название папки не может включать более 255 символов");
        }
    }
}
