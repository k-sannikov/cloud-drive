using Application.CloudDriveIntegrationClient;
using Application.Notes.Repositories;
using Domain;
using Infrastructure.CloudDriveIntegrationClient.RequestDto;

namespace Application.Notes.Services;

public class NoteService : INoteService
{
    private readonly INoteRepositories _apiRepositories;
    private readonly ICloudDriveClient _cloudDriveClient;

    public NoteService(INoteRepositories apiRepositories, ICloudDriveClient cloudDriveClient)
    {
        _apiRepositories = apiRepositories;
        _cloudDriveClient = cloudDriveClient;
    }

    public async Task AddNote(string parentId, Note note)
    {
        note.Id = Guid.NewGuid().ToString();
        CreateNodeDto noteDto = new()
        {
            ParentId = parentId,
            Id = note.Id,
            Name = note.Name,
            Type = "Note",
        };

        bool isCreate = await _cloudDriveClient.CreateNode(noteDto);

        if (!isCreate)
        {
            throw new Exception("Node not created");
        }
        await _apiRepositories.Add(note);
    }

    public async Task DeleteNote(string id)
    {
        bool isDelete = await _cloudDriveClient.DeleteNode(id);

        if (!isDelete)
        {
            throw new Exception("Node not delete");
        }

        await _apiRepositories.Delete(id);
    }

    public async Task<Note> GetNote(string id)
    {
        return await _apiRepositories.Get(id);
    }

    public async Task UpdateNote(Note note)
    {
        bool isRename = await _cloudDriveClient.RenameNode(note.Id, note.Name);

        if (!isRename)
        {
            throw new Exception("Node not rename");
        }
        await _apiRepositories.Update(note);
    }
}