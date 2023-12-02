using Application.FileSystem;
using CloudDrive.Auth;
using CloudDrive.Dto.Extensions;
using CloudDrive.Dto.NodesDto;
using CloudDrive.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers
{
    /// <summary>
    /// Внетреннее API для взаимодействия с внешними микросервисами
    /// </summary>
    [ApiController]
    [ApiKeyAuthFilter]
    [Route("api/v{version:apiVersion}/internal")]
    [ApiVersion("1.0")]
    public class InternalController : ControllerBase
    {
        private readonly IFileSystemService _fileSystemService;

        public InternalController(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }

        /// <summary>
        /// Создать ноду
        /// </summary>
        [HttpPost]
        [Route("nodes")]
        public async Task<IActionResult> CreateNode([FromBody] CreateNodeDto body)
        {
            try
            {
                await _fileSystemService.CreateNode(body.ParentId, body.ToDomain());
            }
            catch (Exception exception)
            {
                return BadRequest(new ErrorResponse(exception.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Переименовать ноду
        /// <param name="nodeId" example="b6a4ca9f-5d2d-440b-8d59-5a04be50ea60">
        /// Id ноды
        /// </summary>
        [HttpPut]
        [Route("nodes/{nodeId}")]
        public async Task<IActionResult> RenameNode([FromRoute] string nodeId, [FromBody] RenameNodeDto body)
        {
            try
            {
                await _fileSystemService.RenameNode(nodeId, body.Name);
            }
            catch (Exception exception)
            {
                return BadRequest(new ErrorResponse(exception.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Удалить ноду
        /// <param name="nodeId" example="b6a4ca9f-5d2d-440b-8d59-5a04be50ea60">
        /// Id ноды
        /// </summary>
        [HttpDelete]
        [Route("nodes/{nodeId}")]
        public async Task<IActionResult> DeleteNode([FromRoute] string nodeId)
        {
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
}
