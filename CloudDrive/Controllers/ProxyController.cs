using Application.AccessSystem;
using Application.ProxyServices;
using CloudDrive.Dto.NodesDto;
using CloudDrive.Dto.ProxyDto;
using CloudDrive.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace CloudDrive.Controllers
{
    /// <summary>
    /// API для проксирования запроса в внешние микросервисы
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/proxy")]
    [ApiVersion("1.0")]
    public class ProxyController : ControllerBase
    {
        private readonly IAccessService _accessService;
        private readonly IProxyService _proxyService;
        private readonly ProxySettings _proxySettings;

        public ProxyController(IAccessService accessService, IProxyService proxyService, ProxySettings proxySettings)
        {
            _accessService = accessService;
            _proxyService = proxyService;
            _proxySettings = proxySettings;
        }

        /// <summary>
        /// Проксирование запроса в внешний сервис
        /// </summary>
        /// <param name="key" example="create-note">Ключ маршрута для перенаправления запроса</param>
        /// <param name="nodeId" example="b6a4ca9f-5d2d-440b-8d59-5a04be50ea60">Id ноды</param>
        [HttpPost]
        [Route("{key}/{nodeId}")]
        public async Task<IActionResult> Proxy([FromRoute] string key,
            [FromRoute] string nodeId,
            [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] JsonElement body)
        {
            string userId = User.GetUserId();

            bool hasAccess = await _accessService.HasAccess(userId, nodeId);

            if (!hasAccess)
            {
                return Forbid();
            }

            string json = string.Empty;

            if (body.ValueKind != JsonValueKind.Undefined)
            {
                json = body.GetRawText();
            }

            HttpResponseMessage response;

            try
            {
                response = _proxyService.ResoveRequest(key, nodeId, json);
            }
            catch (Exception exception)
            {
                return BadRequest(new ErrorResponse(exception.Message));
            }

            string responseJson = await response.Content.ReadAsStringAsync();

            Response.Headers.Add("Content-Type", "application/json");
            return StatusCode((int)response.StatusCode, responseJson);
        }

        /// <summary>
        /// Получить конфигурацию внешних сервисов
        /// </summary>
        [HttpGet]
        [Route("configuration")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<ProxyServiceDto>),
            description: "Конфигурация внешних сервисов")]
        public IActionResult GetProxyConfiguration()
        {
            List<ProxyServiceDto> settings = new();

            foreach (ProxyServiceSettings service in _proxySettings.Services)
            {
                ProxyServiceDto settingsDto = new()
                {
                    UiUrl = service.UiUrl,
                    Label = service.Label,
                    Icon = service.Icon,
                    Type = service.Type,
                };

                settings.Add(settingsDto);
            }
            return Ok(settings);
        }
    }
}
