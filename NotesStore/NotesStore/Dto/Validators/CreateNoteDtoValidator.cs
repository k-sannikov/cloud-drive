using FluentValidation;

namespace NotesStore.Dto.Validators;

public class CreateNoteDtoValidator : AbstractValidator<CreateNoteDto>
{
    public CreateNoteDtoValidator()
    {
        RuleFor(note => note.Name)
            .NotEmpty()
            .MaximumLength(80);

        RuleFor(note => note.Description)
            .NotEmpty()
            .MaximumLength(255);
    }
}