using CloudDrive.Dto.FoldersDto;
using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class CreateFolderDtoValidator : AbstractValidator<CreateFolderDto>
    {
        public CreateFolderDtoValidator()
        {
            RuleFor(createFolderDto => createFolderDto.ParentId)
                .NotEmpty();

            RuleFor(createFolderDto => createFolderDto.Name)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
