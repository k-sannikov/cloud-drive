using Application.AccessSystem;
using Application.FileSystem;
using CloudDrive.Dto;
using CloudDrive.Utilities;
using Domain.FileSystem;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CloudDrive.Controllers;

[ApiController]
[Authorize]
[Route("api/nodes")]
public class FileSystemController : ControllerBase
{
    private readonly IFileSystemService _fileSystemService;
    private readonly IAccessService _accessService;

    public FileSystemController(IFileSystemService fileSystemService,
        IAccessService accessService)
    {
        _fileSystemService = fileSystemService;
        _accessService = accessService;
    }

    /// <summary>
    /// Получить дочерние ноды
    /// </summary>
    [HttpGet]
    [Route("{nodeId}/childs")]
    public async Task<IActionResult> GetChildsNodes([FromRoute] string nodeId)
    {
        bool hasAccess = await _accessService.HasAccess(User.GetUserId(), nodeId);

        if (!hasAccess)
        {
            return StatusCode(403, new ErrorResponse("No accesses"));
        }

        IReadOnlyList<Node> nodes;
        try
        {
            nodes = await _fileSystemService.GetChildNodes(nodeId);
        }
        catch (Exception exception)
        {
            return BadRequest(new ErrorResponse(exception.Message));
        }

        return Ok(nodes.ToDto(nodeId));
    }

    /// <summary>
    /// Получить все родительские ноды
    /// </summary>
    [HttpGet]
    [Route("{nodeId}/parents")]
    public async Task<IActionResult> GetParentsNodes([FromRoute] string nodeId)
    {
        bool hasAccess = await _accessService.HasAccess(User.GetUserId(), nodeId);

        if (!hasAccess)
        {
            return StatusCode(403, new ErrorResponse("No accesses"));
        }

        IReadOnlyList<Node> nodes;
        try
        {
            nodes = await _fileSystemService.GetParentsNodes(nodeId);
        }
        catch (Exception exception)
        {
            return BadRequest(new ErrorResponse(exception.Message));
        }

        IReadOnlyList<NodeDto> nodesDto = nodes.Select(n => n.ToDto()).ToList();

        return Ok(nodesDto);
    }

    /// <summary>
    /// Удалить ноду со всеми ее дочерними нодами
    /// </summary>
    [HttpDelete]
    [Route("{nodeId}")]
    public async Task<IActionResult> DeleteNode([FromRoute] string nodeId)
    {
        bool hasAccess = await _accessService.HasAccess(User.GetUserId(), nodeId);

        if (!hasAccess)
        {
            return Forbid();
        }

        try
        {
            await _fileSystemService.DeleteNode(nodeId);
        }
        catch (Exception exception)
        {

            return BadRequest(new ErrorResponse(exception.Message));
        }

        return Ok();
    }
}
