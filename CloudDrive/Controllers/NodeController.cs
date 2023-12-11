using Application.AccessSystem;
using Application.FileSystem;
using CloudDrive.Dto.Extensions;
using CloudDrive.Dto.NodesDto;
using CloudDrive.Utilities;
using Domain.FileSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CloudDrive.Controllers;

/// <summary>
/// API для упраления нодами
/// </summary>
[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/nodes")]
[ApiVersion("1.0")]
public class NodeController : ControllerBase
{
    private readonly IFileSystemService _fileSystemService;
    private readonly IAccessService _accessService;

    public NodeController(IFileSystemService fileSystemService,
        IAccessService accessService)
    {
        _fileSystemService = fileSystemService;
        _accessService = accessService;
    }

    /// <summary>
    /// Получить дочерние ноды
    /// </summary>
    /// <param name="nodeId" example="b6a4ca9f-5d2d-440b-8d59-5a04be50ea60">Id ноды</param>
    [HttpGet]
    [Route("{nodeId}/childs")]
    [SwaggerResponse(statusCode: 200, type: typeof(NodeListDto),
            description: "Список дочерних нод")]
    public async Task<IActionResult> GetChildsNodes([FromRoute] string nodeId)
    {
        bool hasAccess = await _accessService.HasAccess(User.GetUserId(), nodeId);

        if (!hasAccess)
        {
            return StatusCode(403, new ErrorResponse("Нет доступа"));
        }

        IReadOnlyList<Node> nodes;
        Node node;

        try
        {
            nodes = await _fileSystemService.GetChildNodes(nodeId);
            node = (await _fileSystemService.GetNode<Folder>(nodeId))
                ?? throw new Exception("Корневой узел не найден");
        }
        catch (Exception exception)
        {
            return BadRequest(new ErrorResponse(exception.Message));
        }

        return Ok(nodes.ToDto(node));
    }

    /// <summary>
    /// Получить путь до ноды
    /// </summary>
    /// <param name="nodeId" example="b6a4ca9f-5d2d-440b-8d59-5a04be50ea60">Id ноды</param>
    [HttpGet]
    [Route("{nodeId}/path")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<NodeDto>),
            description: "Список нод (путь до ноды)")]
    public async Task<IActionResult> GetPath([FromRoute] string nodeId)
    {
        bool hasAccess = await _accessService.HasAccess(User.GetUserId(), nodeId);

        if (!hasAccess)
        {
            return StatusCode(403, new ErrorResponse("Нет доступа"));
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
    /// Получить относительный путь до ноды
    /// </summary>
    /// <param name="nodeId" example="b6a4ca9f-5d2d-440b-8d59-5a04be50ea60">Id ноды</param>
    [HttpGet]
    [Route("{nodeId}/relative-path")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<NodeDto>),
            description: "Список нод (путь до ноды)")]
    public async Task<IActionResult> GetRelativePath([FromRoute] string nodeId)
    {
        bool hasAccess = await _accessService.HasAccess(User.GetUserId(), nodeId);

        if (!hasAccess)
        {
            return StatusCode(403, new ErrorResponse("Нет доступа"));
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

        List<NodeDto> nodesDto = new();

        foreach (Node node in nodes)
        {
            if (await _accessService.HasAccess(User.GetUserId(), node.Id))
            {
                nodesDto.Add(node.ToDto());
            }
        }

        return Ok(nodesDto);
    }

    /// <summary>
    /// Удалить ноду со всеми ее дочерними нодами
    /// </summary>
    /// <param name="nodeId" example="b6a4ca9f-5d2d-440b-8d59-5a04be50ea60">Id ноды</param>
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

    /// <summary>
    /// Получить узлы, к которым пользователю выдали доступ
    /// </summary>
    [HttpGet]
    [Route("accessed")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<NodeDto>),
            description: "Список нод к которым пользователю выдали доступ")]
    public async Task<IActionResult> GetAvailableNodes()
    {
        IReadOnlyList<Node> nodes;
        try
        {
            nodes = await _fileSystemService.GetAvailableNodes(User.GetUserId());
        }
        catch (Exception exception)
        {
            return BadRequest(new ErrorResponse(exception.Message));
        }

        IReadOnlyList<NodeDto> nodesDto = nodes.Select(n => n.ToDto()).ToList();

        return Ok(nodesDto);
    }
}
