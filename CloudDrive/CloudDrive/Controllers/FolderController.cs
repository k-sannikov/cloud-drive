using Application.Folders;
using CloudDrive.Dto;
using CloudDrive.Utilities;
using Domain.FileSystem;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers;

[ApiController]
[Route("api/folders")]
public class FolderController : ControllerBase
{
    private readonly IFoldersService _foldersService;
    private readonly IValidator<CreateFolderDto> _createFolderDtoValidator;

    public FolderController(IFoldersService foldersService, IValidator<CreateFolderDto> createFolderDtoValidator)
    {
        _foldersService = foldersService;
        _createFolderDtoValidator = createFolderDtoValidator;
    }

    /// <summary>
    /// Получить родительскую директорию текущего пользователя (NotImplemented)
    /// </summary>
    [HttpGet]
    [Route("root")]
    public async Task<IActionResult> GetRootFolder()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Создать директорию
    /// </summary>
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateFolder([FromBody] CreateFolderDto body)
    {
        ValidationResult validationResult = await _createFolderDtoValidator.ValidateAsync(body);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse(validationResult.ToDictionary()));
        }

        Folder folder = body.ToDomain();

        try
        {
            await _foldersService.CreateFolder(folder, body.ParentId);
        }
        catch (Exception exeption)
        {

            return BadRequest(new ErrorResponse(exeption.Message));
        }

        return Ok();
    }
}
