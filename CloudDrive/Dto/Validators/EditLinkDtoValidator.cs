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
                .WithMessage("название сслыки не может быть пустым")
                .MaximumLength(60)
                .WithMessage("название ссылки не может включать более 60 символов");

            RuleFor(editLinkDto => editLinkDto.Description)
                .MaximumLength(255)
                .WithMessage("описание ссылки не может включать более 255 символов");

            RuleFor(editLinkDto => editLinkDto.Url)
                .NotEmpty()
                .WithMessage("адрес сслыки не может быть пустым")
                .MaximumLength(255)
                .WithMessage("адрес ссылки не может включать более 255 символов");
        }
    }
}
