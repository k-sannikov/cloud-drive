using Application.AccessSystem;
using Application.FileSystem;
using Application.Folders;
using CloudDrive.Dto.AccessesDto;
using CloudDrive.Dto.Extensions;
using CloudDrive.Dto.FoldersDto;
using CloudDrive.Dto.NodesDto;
using CloudDrive.Utilities;
using Domain.AccessSystem;
using Domain.FileSystem;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CloudDrive.Controllers;

/// <summary>
/// API управления директориями
/// </summary>
[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/folders")]
[ApiVersion("1.0")]
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
    /// Получить корневую директорию текущего пользователя
    /// </summary>
    [HttpGet]
    [Route("root")]
    [SwaggerResponse(statusCode: 200, type: typeof(NodeListDto),
            description: "Список нод корневой директории пользователя")]
    public async Task<IActionResult> GetRootFolder()
    {
        Access access = await _accessService.GetRootAccess(User.GetUserId());

        if (access is null)
        {
            return StatusCode(403, new ErrorResponse("No accesses"));
        }

        IReadOnlyList<Node> nodes;
        Node node;

        try
        {
            nodes = await _fileSystemService.GetChildNodes(access.NodeId);
            node = (await _fileSystemService.GetNode<Folder>(access.NodeId))
                ?? throw new Exception("Root node not found");
        }
        catch (Exception exception)
        {
            return BadRequest(new ErrorResponse(exception.Message));
        }

        return Ok(nodes.ToDto(node));
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
    /// <param name="nodeId" example="b6a4ca9f-5d2d-440b-8d59-5a04be50ea60">
    /// Id ноды
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
