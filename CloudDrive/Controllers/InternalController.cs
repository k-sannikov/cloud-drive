using Application.FileSystem;
using CloudDrive.Auth;
using CloudDrive.Dto.Extensions;
using CloudDrive.Dto.NodesDto;
using CloudDrive.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers
{
    [ApiController]
    [ApiKeyAuthFilter]
    [Route("api/internal")]
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
        /// </summary>
        [HttpPut]
        [Route("nodes/{id}")]
        public async Task<IActionResult> RenameNode([FromRoute] string id, [FromBody] RenameNodeDto body)
        {
            try
            {
                await _fileSystemService.RenameNode(id, body.Name);
            }
            catch (Exception exception)
            {
                return BadRequest(new ErrorResponse(exception.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Удалить ноду
        /// </summary>
        [HttpDelete]
        [Route("nodes/{id}")]
        public async Task<IActionResult> DeleteNode([FromRoute] string id)
        {
            try
            {
                await _fileSystemService.DeleteNode(id);
            }
            catch (Exception exception)
            {
                return BadRequest(new ErrorResponse(exception.Message));
            }

            return Ok();
        }
    }
}
