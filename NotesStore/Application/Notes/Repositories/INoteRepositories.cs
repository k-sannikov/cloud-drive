using Domain;

namespace Application.Notes.Repositories;
public interface INoteRepositories
{
    public Task Add(Note note);
    public Task Delete(string id);
    public Task Update(Note note);
    public Task<Note> Get(string id);
}
