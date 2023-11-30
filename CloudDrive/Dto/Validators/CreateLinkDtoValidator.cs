using CloudDrive.Dto.LinksDto;
using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class CreateLinkDtoValidator : AbstractValidator<CreateLinkDto>
    {
        public CreateLinkDtoValidator()
        {
            RuleFor(createLinkDto => createLinkDto.ParentId)
                .NotEmpty();

            RuleFor(createLinkDto => createLinkDto.Name)
                .NotEmpty()
                .MaximumLength(60);

            RuleFor(createLinkDto => createLinkDto.Description)
                .MaximumLength(255);

            RuleFor(createLinkDto => createLinkDto.Url)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
