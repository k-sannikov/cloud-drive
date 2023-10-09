using Application.FileSystem;
using CloudDrive.Dto;
using CloudDrive.Utilities;
using Domain.FileSystem;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers;

[ApiController]
[Route("api/links")]
public class LinkController : ControllerBase
{
    private readonly IFileSystemService _fileSystemService;
    private readonly IValidator<CreateLinkDto> _createLinkDtoValidator;
    private readonly IValidator<EditLinkDto> _editLinkDtoValidator;

    public LinkController(IFileSystemService fileSystemService,
        IValidator<CreateLinkDto> createLinkDtoValidator,
        IValidator<EditLinkDto> editLinkDtoValidator)
    {
        _fileSystemService = fileSystemService;
        _createLinkDtoValidator = createLinkDtoValidator;
        _editLinkDtoValidator = editLinkDtoValidator;
    }

    /// <summary>
    /// Получить ссылку
    /// </summary>
    [HttpGet]
    [Route("{nodeId}")]
    public async Task<IActionResult> GetLink([FromRoute] string nodeId)
    {
        Link link;

        try
        {
            link = await _fileSystemService.GetNode<Link>(nodeId);
        }
        catch (Exception exeption)
        {

            return BadRequest(new ErrorResponse(exeption.Message));
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
        catch (Exception exeption)
        {

            return BadRequest(new ErrorResponse(exeption.Message));
        }

        return Ok();
    }

    /// <summary>
    /// Редактировать ссылку
    /// </summary>
    [HttpPut]
    [Route("{nodeId}")]
    public async Task<IActionResult> EditLink([FromRoute] string nodeId, [FromBody] EditLinkDto body)
    {
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
        catch (Exception exeption)
        {

            return BadRequest(new ErrorResponse(exeption.Message));
        }

        return Ok();
    }
}
