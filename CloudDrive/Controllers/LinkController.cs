﻿using Application.AccessSystem;
using Application.FileSystem;
using CloudDrive.Dto.Extensions;
using CloudDrive.Dto.LinksDto;
using CloudDrive.Dto.NodesDto;
using CloudDrive.Utilities;
using Domain.FileSystem;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CloudDrive.Controllers;

/// <summary>
/// API для упраления ссылками
/// </summary>
[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/links")]
[ApiVersion("1.0")]
public class LinkController : ControllerBase
{
    private readonly IFileSystemService _fileSystemService;
    private readonly IAccessService _accessService;
    private readonly IValidator<CreateLinkDto> _createLinkDtoValidator;
    private readonly IValidator<EditLinkDto> _editLinkDtoValidator;

    public LinkController(IFileSystemService fileSystemService,
        IAccessService accessService,
        IValidator<CreateLinkDto> createLinkDtoValidator,
        IValidator<EditLinkDto> editLinkDtoValidator)
    {
        _fileSystemService = fileSystemService;
        _accessService = accessService;
        _createLinkDtoValidator = createLinkDtoValidator;
        _editLinkDtoValidator = editLinkDtoValidator;
    }

    /// <summary>
    /// Получить ссылку
    /// </summary>
    /// <param name="nodeId" example="b6a4ca9f-5d2d-440b-8d59-5a04be50ea60">Id ноды</param>
    [HttpGet]
    [Route("{nodeId}")]
    [SwaggerResponse(statusCode: 200, type: typeof(LinkDto),
            description: "Информаци о ссылке")]
    public async Task<IActionResult> GetLink([FromRoute] string nodeId)
    {
        bool hasAccess = await _accessService.HasAccess(User.GetUserId(), nodeId);

        if (!hasAccess)
        {
            return Forbid();
        }

        Link link;

        try
        {
            link = await _fileSystemService.GetNode<Link>(nodeId);
        }
        catch (Exception exception)
        {
            return BadRequest(new ErrorResponse(exception.Message));
        }

        LinkDto linkDto = link.ToDto();

        return Ok(linkDto);
    }

    /// <summary>
    /// Создать ссылку
    /// </summary>
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateLink([FromBody] CreateLinkDto body)
    {
        bool hasAccess = await _accessService.HasAccess(User.GetUserId(), body.ParentId);

        if (!hasAccess)
        {
            return Forbid();
        }

        ValidationResult validationResult = await _createLinkDtoValidator.ValidateAsync(body);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse(validationResult.ToDictionary()));
        }

        Link link = body.ToDomain();

        try
        {
            await _fileSystemService.CreateNode(body.ParentId, link);
        }
        catch (Exception exception)
        {

            return BadRequest(new ErrorResponse(exception.Message));
        }

        return Ok();
    }

    /// <summary>
    /// Редактировать ссылку
    /// </summary>
    /// <param name="nodeId" example="b6a4ca9f-5d2d-440b-8d59-5a04be50ea60">Id ноды</param>
    [HttpPut]
    [Route("{nodeId}")]
    public async Task<IActionResult> EditLink([FromRoute] string nodeId, [FromBody] EditLinkDto body)
    {
        bool hasAccess = await _accessService.HasAccess(User.GetUserId(), nodeId);

        if (!hasAccess)
        {
            return Forbid();
        }

        ValidationResult validationResult = await _editLinkDtoValidator.ValidateAsync(body);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse(validationResult.ToDictionary()));
        }

        Link link = body.ToDomain(nodeId);

        try
        {
            await _fileSystemService.EditNode<Link>(link);
        }
        catch (Exception exception)
        {

            return BadRequest(new ErrorResponse(exception.Message));
        }

        return Ok();
    }
}
