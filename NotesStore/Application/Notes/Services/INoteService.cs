using Domain;

namespace Application.Notes.Services;

public interface INoteService
{
    public Task AddNote(string parentId, Note note);
    public Task DeleteNote(string id);
    public Task UpdateNote(Note note);
    public Task<Note> GetNote(string id);
}