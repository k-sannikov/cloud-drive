using CloudDrive.Dto.LinksDto;
using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class EditLinkDtoValidator : AbstractValidator<EditLinkDto>
    {
        public EditLinkDtoValidator()
        {
            RuleFor(editLinkDto => editLinkDto.Name)
                .NotEmpty()
                .MaximumLength(60);

            RuleFor(editLinkDto => editLinkDto.Description)
                .MaximumLength(255);

            RuleFor(editLinkDto => editLinkDto.Url)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
