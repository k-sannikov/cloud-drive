using CloudDrive.Dto.LinksDto;
using FluentValidation;

namespace CloudDrive.Dto.Validators
{
    public class CreateLinkDtoValidator : AbstractValidator<CreateLinkDto>
    {
        public CreateLinkDtoValidator()
        {
            RuleFor(createLinkDto => createLinkDto.ParentId)
                .NotEmpty()
                .WithMessage("id родительской папки не может быть пустым");

            RuleFor(createLinkDto => createLinkDto.Name)
                .NotEmpty()
                .WithMessage("название сслыки не может быть пустым")
                .MaximumLength(60)
                .WithMessage("название ссылки не может включать более 60 символов");

            RuleFor(createLinkDto => createLinkDto.Description)
                .MaximumLength(255)
                .WithMessage("описание ссылки не может включать более 255 символов");

            RuleFor(createLinkDto => createLinkDto.Url)
                .NotEmpty()
                .WithMessage("адрес сслыки не может быть пустым")
                .MaximumLength(255)
                .WithMessage("адрес ссылки не может включать более 255 символов");
        }
    }
}
