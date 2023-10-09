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
    private readonly IValidator<EditNameNodeDto> _editNameNodeDtoValidator;

    public FileSystemController(IFileSystemService fileSystemService, IValidator<EditNameNodeDto> editNameNodeDtoValidator)
    {
        _fileSystemService = fileSystemService;
        _editNameNodeDtoValidator = editNameNodeDtoValidator;
    }


    /// <summary>
    /// Получить дочерние ноды
    /// </summary>
    [HttpGet]
    [Route("{nodeId}/childs")]
    public async Task<IActionResult> GetChildsNodes([FromRoute] string nodeId)
    {
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
