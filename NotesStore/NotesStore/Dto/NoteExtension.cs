using Domain;

namespace NotesStore.Dto;

public static class NoteExtension
{
    public static Note ToDomain(this CreateNoteDto note)
    {
        return new Note()
        {
            Id = Guid.NewGuid().ToString(),
            Name = note.Name,
            Description = note.Description,
        };
    }

    public static Note ToDomain(this UpdateNoteDto note, string id)
    {
        return new Note()
        {
            Id = id,
            Name = note.Name,
            Description = note.Description,
        };
    }
}