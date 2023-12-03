using Application.AccessSystem;
using Application.Auth;
using Application.Foundations;
using CloudDrive.Dto.AccessesDto;
using CloudDrive.Dto.Extensions;
using CloudDrive.Utilities;
using Domain.AccessSystem;
using Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CloudDrive.Controllers
{
    /// <summary>
    /// API для управления доступом к нодам
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/access")]
    [ApiVersion("1.0")]
    public class AccessController : ControllerBase
    {
        private readonly IAccessService _accessService;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public AccessController(IAccessService accessService, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _accessService = accessService;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Получить список пользователей у которых есть доступ к ноде
        /// </summary>
        /// <param name="nodeId" example="b6a4ca9f-5d2d-440b-8d59-5a04be50ea60">Id ноды</param>
        [HttpGet]
        [Route("{nodeId}")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<AccessDto>),
            description: "Список пользователей у которых есть доступ к ноде")]
        public async Task<IActionResult> GetAccessesByNodeId([FromRoute] string nodeId)
        {
            bool hasAccess = await _accessService.HasAccess(User.GetUserId(), nodeId);

            if (!hasAccess)
            {
                return StatusCode(403, new ErrorResponse("No accesses"));
            }

            List<Access> accesses = await _accessService.GetByNodeId(nodeId);

            accesses = accesses.Where(a => !a.IsOwner).ToList();

            List<AccessDto> accessesDto = accesses.Select(a => a.ToDto()).ToList();

            return Ok(accessesDto);
        }

        /// <summary>
        /// Выдать доступ к ноде
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAccess([FromBody] CreateAccessDto body)
        {
            Access access = await _accessService.GetAccess(User.GetUserId(), body.NodeId);

            if (access is null)
            {
                return StatusCode(403, new ErrorResponse("No accesses"));
            }

            if (!access.IsOwner)
            {
                return StatusCode(403, new ErrorResponse("You are not the owner node"));
            }

            User user = await _authService.GetUserByUsername(body.Username);

            if (user is null)
            {
                return BadRequest(new ErrorResponse("A user with this name does not exist"));
            }

            if (User.GetUserId() == user.Id)
            {
                return BadRequest(new ErrorResponse("You can't give your rights to yourself"));
            }

            Access newAccess = body.ToDomain(user.Id);

            try
            {
                await _accessService.AddAccess(newAccess);
                _unitOfWork.Commit();
            }
            catch (Exception exception)
            {
                return BadRequest(new ErrorResponse(exception.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Удалить доступ к ноде
        /// </summary>
        /// <param name="accessId" example="b6a4ca9f-5d2d-440b-8d59-5a04be50ea60">Id доступа к ноде</param>
        [HttpDelete]
        [Route("{accessId}")]
        public async Task<IActionResult> DeleteAccess([FromRoute] int accessId)
        {
            Access access = await _accessService.GetAccess(accessId);

            if (access is null)
            {
                return StatusCode(403, new ErrorResponse("No accesses"));
            }

            bool hasAccess = await _accessService.HasAccess(User.GetUserId(), access.NodeId);

            if (!hasAccess)
            {
                return StatusCode(403, new ErrorResponse("No accesses"));
            }

            await _accessService.DeleteAccess(accessId);
            _unitOfWork.Commit();

            return Ok();
        }
    }
}
