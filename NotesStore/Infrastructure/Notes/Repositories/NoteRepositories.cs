using Application.Notes.Repositories;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Notes.Repositories;

public class NoteRepositories : INoteRepositories
{
    private readonly DbSet<Note> _notes;

    public NoteRepositories(AppDbContext context)
    {
        _notes = context.Set<Note>();
    }

    public async Task Add(Note note)
    {
        await _notes.AddAsync(note);
    }

    public async Task Delete(string id)
    {
        _notes.Remove(await this.Get(id));
    }

    public async Task<Note> Get(string id)
    {
        return await _notes.FindAsync(id);
    }

    public async Task<List<Note>> GetAll()
    {
        return await _notes.ToListAsync();
    }

    public async Task Update(Note note)
    {
        Note updatedNote = await _notes.FindAsync(note.Id);
        updatedNote.Description = note.Description;
        updatedNote.Name = note.Name;
    }
}