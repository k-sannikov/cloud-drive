using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class CreateLinkDtoValidator : AbstractValidator<CreateLinkDto>
    {
        public CreateLinkDtoValidator()
        {
            RuleFor(createFolderDto => createFolderDto.ParentId)
                .NotEmpty();

            RuleFor(createFolderDto => createFolderDto.Name)
                .NotEmpty()
                .MaximumLength(60);

            RuleFor(createFolderDto => createFolderDto.Description)
                .MaximumLength(255);

            RuleFor(createFolderDto => createFolderDto.Url)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
