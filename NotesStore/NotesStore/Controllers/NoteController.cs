using Application.Foundations;
using Application.Notes.Services;
using NotesStore.Authentication;
using NotesStore.Utilities;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using NotesStore.Dto;
using Microsoft.AspNetCore.Mvc;

namespace NotesStore.Controllers;

[ApiController]
[ApiKeyAuthFilter]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateNoteDto> _createNoteDtoValidator;
    private readonly IValidator<UpdateNoteDto> _updateNoteDtoValidator;

    public NoteController(INoteService noteService,
        IUnitOfWork unitOfWork,
        IValidator<CreateNoteDto> createNoteDtoValidator,
        IValidator<UpdateNoteDto> updateNoteDtoValidator)
    {
        _noteService = noteService;
        _unitOfWork = unitOfWork;
        _createNoteDtoValidator = createNoteDtoValidator;
        _updateNoteDtoValidator = updateNoteDtoValidator;
    }

    [HttpPost]
    [Route("notes/{parentId}")]
    public async Task<IActionResult> AddNote([FromRoute] string parentId, [FromBody] CreateNoteDto note)
    {
        ValidationResult validationResult = await _createNoteDtoValidator.ValidateAsync(note);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse(validationResult.ToDictionary()));
        }

        Note newNote = note.ToDomain();

        try
        {
            await _noteService.AddNote(parentId, newNote);
        }
        catch (Exception exception)
        {
            return BadRequest(new ErrorResponse(exception.Message));
        }
        _unitOfWork.Commit();

        return Ok();
    }

    [HttpGet]
    [Route("notes/{id}")]
    public async Task<IActionResult> GetNote([FromRoute] string id)
    {
        Note receivedNote = await _noteService.GetNote(id);

        if (receivedNote is null)
        {
            return BadRequest(new ErrorResponse("Note not exist"));
        }

        try
        {
            return Ok(receivedNote);
        }
        catch (Exception exception)
        {
            return BadRequest(new ErrorResponse(exception.Message));
        }
    }

    [HttpDelete]
    [Route("notes/{id}")]
    public async Task<IActionResult> DeleteNote([FromRoute] string id)
    {
        try
        {
            await _noteService.DeleteNote(id);
        }
        catch (Exception exception)
        {
            return BadRequest(new ErrorResponse(exception.Message));
        }
        _unitOfWork.Commit();
        return Ok();
    }

    [HttpPut]
    [Route("notes/{id}")]
    public async Task<IActionResult> UpdateNote([FromRoute] string id, [FromBody] UpdateNoteDto note)
    {
        ValidationResult validationResult = await _updateNoteDtoValidator.ValidateAsync(note);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse(validationResult.ToDictionary()));
        }

        Note updatedNote = note.ToDomain(id);

        try
        {
            await _noteService.UpdateNote(updatedNote);
        }
        catch (Exception exception)
        {
            return BadRequest(new ErrorResponse(exception.Message));
        }
        _unitOfWork.Commit();

        return Ok();
    }
}