using Application.AccessSystem;
using Application.FileSystem;
using Application.Folders;
using CloudDrive.Dto;
using CloudDrive.Utilities;
using Domain.AccessSystem;
using Domain.FileSystem;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers;

[ApiController]
[Authorize]
[Route("api/folders")]
public class FolderController : ControllerBase
{
    private readonly IFoldersService _foldersService;
    private readonly IAccessService _accessService;
    private readonly IFileSystemService _fileSystemService;
    private readonly IValidator<CreateFolderDto> _createFolderDtoValidator;
    private readonly IValidator<EditFolderDto> _editFolderDtoValidator;

    public FolderController(IFoldersService foldersService,
        IAccessService accessService,
        IFileSystemService fileSystemService,
        IValidator<CreateFolderDto> createFolderDtoValidator,
        IValidator<EditFolderDto> editFolderDtoValidator)
    {
        _foldersService = foldersService;
        _accessService = accessService;
        _fileSystemService = fileSystemService;
        _createFolderDtoValidator = createFolderDtoValidator;
        _editFolderDtoValidator = editFolderDtoValidator;
    }


    /// <summary>
    /// Получить родительскую директорию текущего пользователя
    /// </summary>
    [HttpGet]
    [Route("root")]
    public async Task<IActionResult> GetRootFolder()
    {
        Access access = await _accessService.GetRootAccess(User.GetUserId());

        if (access is null)
        {
            return StatusCode(403, new ErrorResponse("No accesses"));
        }

        IReadOnlyList<Node> nodes;

        try
        {
            nodes = await _fileSystemService.GetChildNodes(access.NodeId);
        }
        catch (Exception exception)
        {
            return BadRequest(new ErrorResponse(exception.Message));
        }

        return Ok(nodes.ToDto(access.NodeId));
    }

    /// <summary>
    /// Создать директорию
    /// </summary>
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateFolder([FromBody] CreateFolderDto body)
    {
        bool hasAccess = await _accessService.HasAccess(User.GetUserId(), body.ParentId);

        if (!hasAccess)
        {
            return StatusCode(403, new ErrorResponse("No accesses"));
        }

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
        catch (Exception exception)
        {

            return BadRequest(new ErrorResponse(exception.Message));
        }

        return Ok();
    }

    /// <summary>
    /// Редактировать директорию
    /// </summary>
    [HttpPut]
    [Route("{nodeId}")]
    public async Task<IActionResult> EditFolder([FromRoute] string nodeId, [FromBody] EditFolderDto body)
    {
        bool hasAccess = await _accessService.HasAccess(User.GetUserId(), nodeId);

        if (!hasAccess)
        {
            return StatusCode(403, new ErrorResponse("No accesses"));
        }

        ValidationResult validationResult = await _editFolderDtoValidator.ValidateAsync(body);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse(validationResult.ToDictionary()));
        }

        Folder folder = body.ToDomain(nodeId);

        try
        {
            await _fileSystemService.EditNode<Folder>(folder);
        }
        catch (Exception exception)
        {

            return BadRequest(new ErrorResponse(exception.Message));
        }

        return Ok();
    }
}
