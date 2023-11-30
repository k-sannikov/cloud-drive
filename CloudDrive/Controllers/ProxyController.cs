using Application.AccessSystem;
using Application.ProxyServices;
using CloudDrive.Dto.ProxyDto;
using CloudDrive.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace CloudDrive.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api")]
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

        [HttpPost]
        [Route("proxy/{key}/{id}")]
        public async Task<IActionResult> Proxy([FromRoute] string key,
            [FromRoute] string id,
            [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] JsonElement body)
        {
            string userId = User.GetUserId();

            bool hasAccess = await _accessService.HasAccess(userId, id);

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
                response = _proxyService.ResoveRequest(key, id, json);
            }
            catch (Exception exception)
            {
                return BadRequest(new ErrorResponse(exception.Message));
            }

            string responseJson = await response.Content.ReadAsStringAsync();

            Response.Headers.Add("Content-Type", "application/json");
            return StatusCode((int)response.StatusCode, responseJson);
        }

        [HttpGet]
        [Route("proxy/settings")]
        public IActionResult GetProxySettings()
        {
            List<ProxyServiceDto> settings = new();

            foreach (ProxyServiceSettings service in _proxySettings.Services)
            {
                ProxyServiceDto settingsDto = new()
                {
                    UiUrl = service.UiUrl,
                    Label = service.Label,
                    Icon = service.Icon,
                };

                settings.Add(settingsDto);
            }
            return Ok(settings);
        }
    }
}
