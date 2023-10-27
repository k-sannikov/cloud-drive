using Application.AccessSystem;
using Application.FileSystem;
using CloudDrive.Dto;
using CloudDrive.Utilities;
using Domain.FileSystem;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers;

[ApiController]
[Route("api/nodes")]
public class FileSystemController : ControllerBase
{
    private readonly IFileSystemService _fileSystemService;
    private readonly IAccessService _accessService;
    private readonly IValidator<EditNameNodeDto> _editNameNodeDtoValidator;

    private const int _userId = 0;

    public FileSystemController(IFileSystemService fileSystemService,
        IAccessService accessService,
        IValidator<EditNameNodeDto> editNameNodeDtoValidator)
    {
        _fileSystemService = fileSystemService;
        _accessService = accessService;
        _editNameNodeDtoValidator = editNameNodeDtoValidator;
    }


    /// <summary>
    /// Получить дочерние ноды
    /// </summary>
    [HttpGet]
    [Route("{nodeId}/childs")]
    public async Task<IActionResult> GetChildsNodes([FromRoute] string nodeId)
    {
        bool hasAccess = await _accessService.HasAccess(_userId, nodeId);

        if (!hasAccess)
        {
            return Forbid();
        }

        IReadOnlyList<Node> nodes = await _fileSystemService.GetChildNodes(nodeId);

        IReadOnlyList<NodeDto> nodesDto = nodes.Select(n => n.ToDto()).ToList();

        return Ok(nodesDto);
    }

    /// <summary>
    /// Получить все родительские ноды
    /// </summary>
    [HttpGet]
    [Route("{nodeId}/parents")]
    public async Task<IActionResult> GetParentsNodes([FromRoute] string nodeId)
    {
        bool hasAccess = await _accessService.HasAccess(_userId, nodeId);

        if (!hasAccess)
        {
            return Forbid();
        }

        IReadOnlyList<Node> nodes;
        try
        {
            nodes = await _fileSystemService.GetParentsNodes(nodeId);
        }
        catch (Exception exeption)
        {

            return BadRequest(new ErrorResponse(exeption.Message));
        }

        IReadOnlyList<NodeDto> nodesDto = nodes.Select(n => n.ToDto()).ToList();

        return Ok(nodesDto);
    }

    /// <summary>
    /// Переименовать ноду
    /// </summary>
    [HttpPut]
    [Route("{nodeId}")]
    public async Task<IActionResult> RenameNode([FromRoute] string nodeId, [FromBody] EditNameNodeDto body)
    {
        bool hasAccess = await _accessService.HasAccess(_userId, nodeId);

        if (!hasAccess)
        {
            return Forbid();
        }

        ValidationResult validationResult = await _editNameNodeDtoValidator.ValidateAsync(body);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse(validationResult.ToDictionary()));
        }

        Node node = body.ToDomain(nodeId);

        try
        {
            await _fileSystemService.RenameNode(node);
        }
        catch (Exception exeption)
        {

            return BadRequest(new ErrorResponse(exeption.Message));
        }

        return Ok();
    }

    /// <summary>
    /// Удалить ноду со всеми ее дочерними нодами
    /// </summary>
    [HttpDelete]
    [Route("{nodeId}")]
    public async Task<IActionResult> DeleteNode([FromRoute] string nodeId)
    {
        bool hasAccess = await _accessService.HasAccess(_userId, nodeId);

        if (!hasAccess)
        {
            return Forbid();
        }

        try
        {
            await _fileSystemService.DeleteNode(nodeId);
        }
        catch (Exception exeption)
        {

            return BadRequest(new ErrorResponse(exeption.Message));
        }

        return Ok();
    }
}
