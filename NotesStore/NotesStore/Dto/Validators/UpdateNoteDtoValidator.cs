using FluentValidation;

namespace NotesStore.Dto.Validators;

public class UpdateNoteDtoValidator : AbstractValidator<UpdateNoteDto>
{
    public UpdateNoteDtoValidator()
    {
        RuleFor(note => note.Name)
            .NotEmpty()
            .MaximumLength(80);

        RuleFor(note => note.Description)
            .NotEmpty()
            .MaximumLength(255);
    }
}